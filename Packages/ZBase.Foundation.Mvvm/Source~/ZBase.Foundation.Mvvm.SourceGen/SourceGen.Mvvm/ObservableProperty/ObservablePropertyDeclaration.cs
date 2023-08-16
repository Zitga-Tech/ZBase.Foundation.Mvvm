using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm.ObservablePropertySourceGen
{
    public partial class ObservablePropertyDeclaration
    {
        public const string IOBSERVABLE_OBJECT_INTERFACE = "global::ZBase.Foundation.Mvvm.ComponentModel.IObservableObject";
        public const string OBSERVABLE_PROPERTY_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.ComponentModel.ObservablePropertyAttribute";
        public const string NOTIFY_PROPERTY_CHANGED_FOR_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.ComponentModel.NotifyPropertyChangedForAttribute";
        public const string NOTIFY_CAN_EXECUTE_CHANGED_FOR_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.ComponentModel.NotifyCanExecuteChangedForAttribute";
        public const string RELAY_COMMAND_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.Input.RelayCommandAttribute";

        public ClassDeclarationSyntax Syntax { get; }

        public INamedTypeSymbol Symbol { get; }

        public string FullyQualifiedName { get; private set; }

        public bool IsBaseObservableObject { get; }

        public ImmutableArray<MemberRef> MemberRefs { get; }

        public bool HasMemberObservableObject { get; }

        /// <summary>
        /// Key is <c>Field.Name</c>
        /// </summary>
        public Dictionary<string, IPropertySymbol> NotifyPropertyChangedForMap { get; }

        /// <summary>
        /// Key is the command name (<c>Method.Name + "Command"</c>)
        /// </summary>
        public HashSet<string> NotifyCanExecuteChangedForSet { get; }

        public ObservablePropertyDeclaration(
              ClassDeclarationSyntax candidate
            , SemanticModel semanticModel
            , CancellationToken token
        )
        {
            using var memberRefs = ImmutableArrayBuilder<MemberRef>.Rent();
            using var diagnosticBuilder = ImmutableArrayBuilder<DiagnosticInfo>.Rent();

            Syntax = candidate;
            Symbol = semanticModel.GetDeclaredSymbol(candidate, token);
            FullyQualifiedName = Symbol.ToFullName();
            NotifyPropertyChangedForMap = new Dictionary<string, IPropertySymbol>();
            NotifyCanExecuteChangedForSet = new HashSet<string>();
            
            if (Symbol.BaseType != null && Symbol.BaseType.TypeKind == TypeKind.Class)
            {
                if (Symbol.BaseType.ImplementsInterface(IOBSERVABLE_OBJECT_INTERFACE))
                {
                    IsBaseObservableObject = true;
                }
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
                            IsObservableObject = field.Type.ImplementsInterface(IOBSERVABLE_OBJECT_INTERFACE),
                        };

                        if (memberRef.IsObservableObject)
                        {
                            HasMemberObservableObject = true;
                        }

                        var uniqueCommandNames = new HashSet<string>();
                        var notifyPropChangedFors = field.GetAttributes(NOTIFY_PROPERTY_CHANGED_FOR_ATTRIBUTE);

                        foreach (var notifyPropChangedFor in notifyPropChangedFors)
                        {
                            if (notifyPropChangedFor != null
                                && notifyPropChangedFor.ConstructorArguments.Length > 0
                                && notifyPropChangedFor.ConstructorArguments[0].Value is string propName
                            )
                            {
                                fieldToPropertyChanged[field.Name] = propName;
                            }
                        }

                        var notifyCanExecuteChangedFors = field.GetAttributes(NOTIFY_CAN_EXECUTE_CHANGED_FOR_ATTRIBUTE);

                        using var commandNames = ImmutableArrayBuilder<string>.Rent();

                        foreach (var notifyCanExecuteChangedFor in notifyCanExecuteChangedFors)
                        {
                            if (notifyCanExecuteChangedFor != null
                                && notifyCanExecuteChangedFor.ConstructorArguments.Length > 0
                            )
                            {
                                var args = notifyCanExecuteChangedFor.ConstructorArguments;

                                foreach (var arg in args)
                                {
                                    if (arg.Value is string commandName)
                                    {
                                        uniqueCommandNames.Add(commandName);
                                        commandSet.Add(commandName);
                                    }
                                }
                            }

                            commandNames.AddRange(uniqueCommandNames);
                        }

                        memberRef.CommandNames = commandNames.ToImmutable();
                        memberRefs.Add(memberRef);
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

            MemberRefs = memberRefs.ToImmutable();

            foreach (var memberRef in MemberRefs)
            {
                memberRef.Member.GatherForwardedAttributes(
                      semanticModel
                    , token
                    , diagnosticBuilder
                    , out var propertyAttributes
                    , DiagnosticDescriptors.InvalidFieldOrPropertyTargetedAttributeOnRelayCommandMethod
                );

                memberRef.ForwardedPropertyAttributes = propertyAttributes;
            }
        }

        public class MemberRef
        {
            public IFieldSymbol Member { get; set; }

            public string PropertyName { get; set; }

            public bool IsObservableObject { get; set; }

            public ImmutableArray<string> CommandNames { get; set; }

            public ImmutableArray<AttributeInfo> ForwardedPropertyAttributes { get; set; }
        }
    }
}
