using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    public partial class ObservablePropertyDeclaration
    {
        public const string IOBSERVABLE_OBJECT_NAME = "IObservableObject";
        public const string IOBSERVABLE_OBJECT_INTERFACE = "global::ZBase.Foundation.Mvvm.IObservableObject";
        public const string OBSERVABLE_PROPERTY_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.ObservablePropertyAttribute";
        public const string NOTIFY_PROPERTY_CHANGED_FOR_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.NotifyPropertyChangedForAttribute";
        public const string NOTIFY_CAN_EXECUTE_CHANGED_FOR_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.NotifyCanExecuteChangedForAttribute";

        public ClassDeclarationSyntax Syntax { get; }

        public string FullyQualifiedName { get; private set; }

        public bool IsValid { get; }

        public List<MemberRef> Members { get; }

        public Dictionary<string, IPropertySymbol> NotifyPropertyChangedForMap { get; }

        public Dictionary<string, IMethodSymbol> NotifyCanExecuteChangedForMap { get; }

        public ObservablePropertyDeclaration(ClassDeclarationSyntax candidate, SemanticModel semanticModel, CancellationToken token)
        {
            var typeSymbol = semanticModel.GetDeclaredSymbol(candidate, token);

            Syntax = candidate;
            FullyQualifiedName = typeSymbol.ToFullName();
            Members = new List<MemberRef>();
            NotifyPropertyChangedForMap = new Dictionary<string, IPropertySymbol>();
            NotifyCanExecuteChangedForMap = new Dictionary<string, IMethodSymbol>();

            var implementInterface = false;

            if (candidate.BaseList != null)
            {
                foreach (var baseType in candidate.BaseList.Types)
                {
                    var typeInfo = semanticModel.GetTypeInfo(baseType.Type, token);

                    if (typeInfo.Type.ToFullName().StartsWith(IOBSERVABLE_OBJECT_INTERFACE))
                    {
                        implementInterface = true;
                        break;
                    }
                }
            }

            if (implementInterface == false)
            {
                return;
            }

            var members = typeSymbol.GetMembers();
            var notifyPropertyChangedForSet = new HashSet<string>();
            var notifyCanExecuteChangedForSet = new HashSet<string>();

            foreach (var member in members)
            {
                if (member is not IFieldSymbol field
                    || field.HasAttribute(OBSERVABLE_PROPERTY_ATTRIBUTE) == false
                )
                {
                    continue;
                }

                var memberRef = new MemberRef {
                    Member = field,
                };

                var notifyPropChangedFor = field.GetAttribute(NOTIFY_PROPERTY_CHANGED_FOR_ATTRIBUTE);

                if (notifyPropChangedFor != null
                    && notifyPropChangedFor.ConstructorArguments.Length > 0
                    && notifyPropChangedFor.ConstructorArguments[0].Value is string propName
                )
                {
                    memberRef.NotifyPropertyChangedFor = propName;
                    notifyPropertyChangedForSet.Add(propName);
                }

                var notifyCanExecuteChangedFor = field.GetAttribute(NOTIFY_CAN_EXECUTE_CHANGED_FOR_ATTRIBUTE);

                if (notifyCanExecuteChangedFor != null
                    && notifyCanExecuteChangedFor.ConstructorArguments.Length > 0
                )
                {
                    var args = notifyCanExecuteChangedFor.ConstructorArguments;

                    foreach (var arg in args)
                    {
                        if (arg.Value is string commandName)
                        {
                            memberRef.NotifyCanExecuteChangedFor.Add(commandName);
                            notifyCanExecuteChangedForSet.Add(commandName);
                        }
                    }
                }

                Members.Add(memberRef);
            }

            foreach (var member in members)
            {
                if (member is IPropertySymbol property)
                {
                    if (notifyPropertyChangedForSet.Contains(property.Name))
                    {
                        NotifyPropertyChangedForMap[property.Name] = property;
                    }

                    continue;
                }
                
                if (member is IMethodSymbol method)
                {
                    var commandName = $"{method.Name}Command";

                    if (notifyCanExecuteChangedForSet.Contains(commandName))
                    {
                        NotifyCanExecuteChangedForMap[commandName] = method;
                    }

                    continue;
                }
            }

            IsValid = Members.Count > 0;
        }

        public class MemberRef
        {
            public IFieldSymbol Member { get; set; }

            public string NotifyPropertyChangedFor { get; set; }

            public HashSet<string> NotifyCanExecuteChangedFor { get; } = new();
        }
    }
}
