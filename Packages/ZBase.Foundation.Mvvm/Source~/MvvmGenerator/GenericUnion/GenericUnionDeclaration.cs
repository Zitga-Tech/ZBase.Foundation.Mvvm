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
        public const string NAMESPACE = "ZBase.Foundation.Mvvm";
        public const string INTERFACE = "global::ZBase.Foundation.Mvvm.Unions.IUnion<";

        public List<StructRef> Structs { get; }

        public List<StructRef> Redundants { get; }

        public GenericUnionDeclaration(
              ImmutableArray<StructRef> candidates
            , Compilation compilation
            , CancellationToken token
        )
        {
            Structs = new List<StructRef>();
            Redundants = new List<StructRef>();

            var filtered = new Dictionary<string, StructRef>();

            foreach (var candidate in candidates)
            {
                var syntaxTree = candidate.Syntax.SyntaxTree;
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                candidate.Symbol = semanticModel.GetDeclaredSymbol(candidate.Syntax, token);

                var typeName = candidate.TypeArgument.ToFullName();

                if (filtered.ContainsKey(typeName) == false)
                {
                    filtered[typeName] = candidate;
                }
                else
                {
                    Redundants.Add(candidate);
                }
            }

            Structs.AddRange(filtered.Values);
        }
    }

    public class StructRef
    {
        public StructDeclarationSyntax Syntax { get; set; }

        public ITypeSymbol Symbol { get; set; }

        public ITypeSymbol TypeArgument { get; set; }
    }
}
