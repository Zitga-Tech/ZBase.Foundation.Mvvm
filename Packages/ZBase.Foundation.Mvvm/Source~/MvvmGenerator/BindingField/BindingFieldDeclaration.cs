using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    public partial class BindingFieldDeclaration
    {
        public const string IBINDER_INTERFACE = "global::ZBase.Foundation.Mvvm.ViewBinding.IBinder";
        public const string BINDING_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.ViewBinding.BindingAttribute";
        public const string UNION_TYPE = "global::ZBase.Foundation.Mvvm.Unions.Union";
        public const string MONO_BEHAVIOUR_TYPE = "global::UnityEngine.MonoBehaviour";

        public ClassDeclarationSyntax Syntax { get; }

        public INamedTypeSymbol Symbol { get; }

        public bool IsBaseBinder { get; }

        public bool ReferenceUnityEngine { get; }

        public ImmutableArray<MemberRef> MemberRefs { get; }

        public ImmutableArray<ITypeSymbol> NonUnionTypes { get; }

        public BindingFieldDeclaration(ClassDeclarationSyntax candidate, SemanticModel semanticModel, CancellationToken token)
        {
            using var memberRefs = ImmutableArrayBuilder<MemberRef>.Rent();

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
                if (member is not IMethodSymbol method
                    || method.Parameters.Length != 1
                )
                {
                    continue;
                }

                var parameter = method.Parameters[0];

                if (parameter.RefKind is not (RefKind.None or RefKind.In))
                {
                    continue;
                }

                var attribute = method.GetAttribute(BINDING_ATTRIBUTE);
                
                if (attribute == null)
                {
                    continue;
                }

                var label = string.Empty;

                foreach (var kv in attribute.NamedArguments)
                {
                    if (kv.Key == "Label" && kv.Value.Value is string lbl)
                    {
                        label = lbl;
                    }
                }

                var argumentType = parameter.Type;
                var isNotUnion = argumentType.ToFullName() != UNION_TYPE;

                memberRefs.Add(new MemberRef {
                    Member = method,
                    Label = label,
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

            using var nonUnionTypes = ImmutableArrayBuilder<ITypeSymbol>.Rent();
            nonUnionTypes.AddRange(filtered.Values);

            MemberRefs = memberRefs.ToImmutable();
            NonUnionTypes = nonUnionTypes.ToImmutable();
        }

        public class MemberRef
        {
            public IMethodSymbol Member { get; set; }

            public string Label { get; set; }

            public ITypeSymbol NonUnionArgumentType { get; set; }
        }
    }
}