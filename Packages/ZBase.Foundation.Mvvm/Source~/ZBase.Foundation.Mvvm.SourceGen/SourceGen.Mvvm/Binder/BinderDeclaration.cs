using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm.BinderSourceGen
{
    public partial class BinderDeclaration
    {
        public const string IBINDER_INTERFACE = "global::ZBase.Foundation.Mvvm.ViewBinding.IBinder";
        public const string BINDING_PROPERTY_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.ViewBinding.BindingPropertyAttribute";
        public const string BINDING_COMMAND_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.ViewBinding.BindingCommandAttribute";
        public const string BINDING_PROPERTY = "global::ZBase.Foundation.Mvvm.ViewBinding.BindingProperty";
        public const string CONVERTER = "global::ZBase.Foundation.Mvvm.ViewBinding.Converter";
        public const string UNION_TYPE = "global::ZBase.Foundation.Mvvm.Unions.Union";
        public const string MONO_BEHAVIOUR_TYPE = "global::UnityEngine.MonoBehaviour";
        private const string IRELAY_COMMAND = "global::ZBase.Foundation.Mvvm.Input.IRelayCommand";

        public ClassDeclarationSyntax Syntax { get; }

        public INamedTypeSymbol Symbol { get; }

        public bool IsBaseBinder { get; }

        public bool ReferenceUnityEngine { get; }

        public ImmutableArray<BindingPropertyRef> BindingPropertyRefs { get; }

        public ImmutableArray<ITypeSymbol> NonUnionTypes { get; }

        public BinderDeclaration(ClassDeclarationSyntax candidate, SemanticModel semanticModel, CancellationToken token)
        {
            using var bindingPropertyRefs = ImmutableArrayBuilder<BindingPropertyRef>.Rent();
            using var diagnosticBuilder = ImmutableArrayBuilder<DiagnosticInfo>.Rent();

            var bindingPropNames = new HashSet<string>();
            var converterNames = new HashSet<string>();
            var relayCommandNames = new HashSet<string>();

            Syntax = candidate;
            Symbol = semanticModel.GetDeclaredSymbol(candidate, token);

            if (Symbol.BaseType != null && Symbol.BaseType.TypeKind == TypeKind.Class)
            {
                if (Symbol.BaseType.ImplementsInterface(IBINDER_INTERFACE))
                {
                    IsBaseBinder = true;
                }
            }

            foreach (var assembly in Symbol.ContainingModule.ReferencedAssemblySymbols)
            {
                if (assembly.ToDisplayString().StartsWith("UnityEngine,"))
                {
                    ReferenceUnityEngine = true;
                    break;
                }
            }

            var members = Symbol.GetMembers();
            var filtered = new Dictionary<string, ITypeSymbol>(members.Length);

            foreach (var member in members)
            {
                if (member is IMethodSymbol method)
                {
                    if (method.Parameters.Length == 1)
                    {
                        var parameter = method.Parameters[0];

                        if (parameter.RefKind is not (RefKind.None or RefKind.In))
                        {
                            continue;
                        }

                        var attribute = method.GetAttribute(BINDING_PROPERTY_ATTRIBUTE);

                        if (attribute == null)
                        {
                            continue;
                        }

                        var argumentType = parameter.Type;
                        var isNotUnion = argumentType.ToFullName() != UNION_TYPE;

                        bindingPropertyRefs.Add(new BindingPropertyRef {
                            Symbol = method,
                            NonUnionArgumentType = isNotUnion ? argumentType : null,
                        });

                        if (isNotUnion == false)
                        {
                            continue;
                        }

                        var typeName = argumentType.ToFullName();

                        if (filtered.ContainsKey(typeName) == false)
                        {
                            filtered[typeName] = argumentType;
                        }
                    }

                    continue;
                }

                if (member is IFieldSymbol field)
                {
                    var typeName = field.Type.ToFullName();
                    
                    if (typeName == BINDING_PROPERTY)
                    {
                        bindingPropNames.Add(field.Name);
                    }
                    else if (typeName == CONVERTER)
                    {
                        converterNames.Add(field.Name);
                    }
                    else if (typeName.StartsWith(IRELAY_COMMAND))
                    {
                        relayCommandNames.Add(field.Name);
                    }
                }
            }

            using var nonUnionTypes = ImmutableArrayBuilder<ITypeSymbol>.Rent();
            nonUnionTypes.AddRange(filtered.Values);

            BindingPropertyRefs = bindingPropertyRefs.ToImmutable();
            NonUnionTypes = nonUnionTypes.ToImmutable();

            foreach (var bindingPropRef in BindingPropertyRefs)
            {
                var bindingPropName = BindingPropertyName(bindingPropRef);
                var converterName = ConverterName(bindingPropRef);
                bindingPropRef.SkipBindingProperty = bindingPropNames.Contains(bindingPropName);
                bindingPropRef.SkipConverter = converterNames.Contains(converterName);

                bindingPropRef.Symbol.GatherForwardedAttributes(
                      semanticModel
                    , token
                    , diagnosticBuilder
                    , out var fieldAttributes
                    , out _
                    , DiagnosticDescriptors.InvalidFieldTargetedAttributeOnBindingMethod
                );

                bindingPropRef.ForwardedFieldAttributes = fieldAttributes;
            }
        }

        public class BindingPropertyRef
        {
            public IMethodSymbol Symbol { get; set; }

            public ITypeSymbol NonUnionArgumentType { get; set; }

            public bool SkipBindingProperty { get; set; }

            public bool SkipConverter { get; set; }

            public ImmutableArray<AttributeInfo> ForwardedFieldAttributes { get; set; }
        }

        public class BindingCommandRef
        {
            public IMethodSymbol Symbol { get; set; }

            public ITypeSymbol NonUnionArgumentType { get; set; }

            public bool SkipBindingProperty { get; set; }

            public bool SkipConverter { get; set; }

            public ImmutableArray<AttributeInfo> ForwardedFieldAttributes { get; set; }
        }
    }
}