using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    public partial class GenericUnionDeclaration
    {
        public const string INTERFACE = "global::ZBase.Foundation.Mvvm.Unions.IUnion<";

        public ImmutableArray<StructRef> StructRefs { get; }

        public ImmutableArray<StructRef> Redundants { get; }

        public GenericUnionDeclaration(
              ImmutableArray<StructRef> candidates
            , Compilation compilation
            , CancellationToken token
        )
        {
            using var redundants = ImmutableArrayBuilder<StructRef>.Rent();
            var filtered = new Dictionary<string, StructRef>();

            foreach (var candidate in candidates)
            {
                var syntaxTree = candidate.Syntax.SyntaxTree;
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                candidate.Symbol = semanticModel.GetDeclaredSymbol(candidate.Syntax, token);

                var typeName = candidate.TypeArgument.ToFullName();

                if (typeName.ToUnionType().IsNativeUnionType() == false
                    && filtered.ContainsKey(typeName) == false
                )
                {
                    filtered[typeName] = candidate;
                }
                else
                {
                    redundants.Add(candidate);
                }
            }

            using var structRefs = ImmutableArrayBuilder<StructRef>.Rent();
            structRefs.AddRange(filtered.Values);

            StructRefs = structRefs.ToImmutable();
            Redundants = redundants.ToImmutable();

        }
    }

    public class StructRef
    {
        public StructDeclarationSyntax Syntax { get; set; }

        public ITypeSymbol Symbol { get; set; }

        public ITypeSymbol TypeArgument { get; set; }
    }
}
