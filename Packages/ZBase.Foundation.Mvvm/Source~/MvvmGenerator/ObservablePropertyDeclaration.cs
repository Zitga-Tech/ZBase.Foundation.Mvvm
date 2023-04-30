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
        public const string RELAY_COMMAND_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.RelayCommandAttribute";

        public ClassDeclarationSyntax Syntax { get; }

        public INamedTypeSymbol Symbol { get; }

        public string FullyQualifiedName { get; private set; }

        public bool IsValid { get; }

        public List<MemberRef> Members { get; }

        /// <summary>
        /// Key is <c>Field.Name</c>
        /// </summary>
        public Dictionary<string, IPropertySymbol> NotifyPropertyChangedForMap { get; }

        /// <summary>
        /// Key is the command name (<c>Method.Name + "Command"</c>)
        /// </summary>
        public HashSet<string> NotifyCanExecuteChangedForSet { get; }

        public ObservablePropertyDeclaration(ClassDeclarationSyntax candidate, SemanticModel semanticModel, CancellationToken token)
        {
            Syntax = candidate;
            Symbol = semanticModel.GetDeclaredSymbol(candidate, token);
            FullyQualifiedName = Symbol.ToFullName();
            Members = new List<MemberRef>();
            NotifyPropertyChangedForMap = new Dictionary<string, IPropertySymbol>();
            NotifyCanExecuteChangedForSet = new HashSet<string>();

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

            var members = Symbol.GetMembers();
            var fieldToPropertyChanged = new Dictionary<string, string>();
            var commandSet = new HashSet<string>();
            var propertyMap = new Dictionary<string, IPropertySymbol>();
            var methods = new List<IMethodSymbol>();

            foreach (var member in members)
            {
                if (member is IFieldSymbol field)
                {
                    if (field.HasAttribute(OBSERVABLE_PROPERTY_ATTRIBUTE))
                    {
                        var memberRef = new MemberRef {
                            Member = field,
                            PropertyName = field.ToPropertyName(),
                        };

                        var notifyPropChangedFor = field.GetAttribute(NOTIFY_PROPERTY_CHANGED_FOR_ATTRIBUTE);

                        if (notifyPropChangedFor != null
                            && notifyPropChangedFor.ConstructorArguments.Length > 0
                            && notifyPropChangedFor.ConstructorArguments[0].Value is string propName
                        )
                        {
                            fieldToPropertyChanged[field.Name] = propName;
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
                                    memberRef.CommandNames.Add(commandName);
                                    commandSet.Add(commandName);
                                }
                            }
                        }

                        Members.Add(memberRef);
                    }

                    continue;
                }

                if (member is IPropertySymbol property)
                {
                    propertyMap[property.Name] = property;
                    continue;
                }

                if (member is IMethodSymbol method && method.Parameters.Length <= 1)
                {
                    if (method.HasAttribute(RELAY_COMMAND_ATTRIBUTE))
                    {
                        methods.Add(method);
                    }

                    continue;
                }
            }

            foreach (var kv in fieldToPropertyChanged)
            {
                var fieldName = kv.Key;
                var propertyName = kv.Value;

                if (propertyMap.TryGetValue(propertyName, out var property))
                {
                    NotifyPropertyChangedForMap[fieldName] = property;
                }
            }

            foreach (var method in methods)
            {
                var commandName = $"{method.Name}Command";

                if (commandSet.Contains(commandName))
                {
                    NotifyCanExecuteChangedForSet.Add(commandName);
                }
            }

            IsValid = Members.Count > 0;
        }

        public class MemberRef
        {
            public IFieldSymbol Member { get; set; }

            public string PropertyName { get; set; }

            public HashSet<string> CommandNames { get; } = new();
        }
    }
}
