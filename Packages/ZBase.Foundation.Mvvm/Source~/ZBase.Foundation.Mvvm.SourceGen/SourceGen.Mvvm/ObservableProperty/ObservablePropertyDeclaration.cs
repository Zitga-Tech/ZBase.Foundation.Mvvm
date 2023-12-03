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

        public ImmutableArray<FieldRef> FieldRefs { get; }

        public ImmutableArray<PropertyRef> PropRefs { get; }

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
            using var fieldRefs = ImmutableArrayBuilder<FieldRef>.Rent();
            using var propRefs = ImmutableArrayBuilder<PropertyRef>.Rent();
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
            var propertyChangedMap = new Dictionary<string, string>();
            var commandSet = new HashSet<string>();
            var propertyMap = new Dictionary<string, IPropertySymbol>();
            var methods = new List<IMethodSymbol>();

            foreach (var member in members)
            {
                if (member is IFieldSymbol field)
                {
                    if (field.HasAttribute(OBSERVABLE_PROPERTY_ATTRIBUTE))
                    {
                        var fieldRef = new FieldRef {
                            Field = field,
                            PropertyName = field.ToPropertyName(),
                            IsObservableObject = field.Type.ImplementsInterface(IOBSERVABLE_OBJECT_INTERFACE),
                        };

                        if (fieldRef.IsObservableObject)
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
                                propertyChangedMap[field.Name] = propName;
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

                        fieldRef.Field.GatherForwardedAttributes(
                              semanticModel
                            , token
                            , diagnosticBuilder
                            , out var propertyAttributes
                            , DiagnosticDescriptors.InvalidPropertyTargetedAttributeOnObservableProperty
                        );

                        fieldRef.ForwardedPropertyAttributes = propertyAttributes;
                        fieldRef.CommandNames = commandNames.ToImmutable();

                        fieldRefs.Add(fieldRef);
                    }

                    continue;
                }

                if (member is IPropertySymbol property)
                {
                    if (property.HasAttribute(OBSERVABLE_PROPERTY_ATTRIBUTE) == false)
                    {
                        propertyMap[property.Name] = property;
                    }
                    else
                    {
                        var propRef = new PropertyRef {
                            Property = property,
                            FieldName = property.ToFieldName(),
                            IsObservableObject = property.Type.ImplementsInterface(IOBSERVABLE_OBJECT_INTERFACE),
                        };

                        if (propRef.IsObservableObject)
                        {
                            HasMemberObservableObject = true;
                        }

                        var uniqueCommandNames = new HashSet<string>();
                        var notifyPropChangedFors = property.GetAttributes(NOTIFY_PROPERTY_CHANGED_FOR_ATTRIBUTE);

                        foreach (var notifyPropChangedFor in notifyPropChangedFors)
                        {
                            if (notifyPropChangedFor != null
                                && notifyPropChangedFor.ConstructorArguments.Length > 0
                                && notifyPropChangedFor.ConstructorArguments[0].Value is string propName
                            )
                            {
                                propertyChangedMap[property.Name] = propName;
                            }
                        }

                        var notifyCanExecuteChangedFors = property.GetAttributes(NOTIFY_CAN_EXECUTE_CHANGED_FOR_ATTRIBUTE);

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

                        propRef.Property.GatherForwardedAttributes(
                              semanticModel
                            , token
                            , diagnosticBuilder
                            , out var fieldAttributes
                            , DiagnosticDescriptors.InvalidFieldMethodTargetedAttributeOnObservableProperty
                        );

                        propRef.ForwardedFieldAttributes = fieldAttributes;
                        propRef.CommandNames = commandNames.ToImmutable();
                        propRefs.Add(propRef);
                    }

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

            foreach (var kv in propertyChangedMap)
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

            FieldRefs = fieldRefs.ToImmutable();
            PropRefs = propRefs.ToImmutable();
        }

        public abstract class MemberRef
        {
            public bool IsObservableObject { get; set; }

            public ImmutableArray<string> CommandNames { get; set; }

            public abstract string GetPropertyName();
        }

        public class FieldRef : MemberRef
        {
            public IFieldSymbol Field { get; set; }

            public string PropertyName { get; set; }

            public ImmutableArray<AttributeInfo> ForwardedPropertyAttributes { get; set; }

            public override string GetPropertyName()
                => PropertyName;
        }

        public class PropertyRef : MemberRef
        {
            public IPropertySymbol Property { get; set; }

            public string FieldName { get; set; }

            public ImmutableArray<AttributeInfo> ForwardedFieldAttributes { get; set; }

            public override string GetPropertyName()
                => Property.Name;
        }
    }
}
