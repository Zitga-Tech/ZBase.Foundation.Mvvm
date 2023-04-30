using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    [Generator]
    public class UnionGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var projectPathProvider = SourceGenHelpers.GetSourceGenConfigProvider(context);

            var candidateProvider = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (node, token) => IsSyntaxMatch(node, token),
                transform: GetSemanticMatch
            ).Where(t => t is { });

            var compilationProvider = context.CompilationProvider;
            var combined = candidateProvider.Combine(compilationProvider).Combine(projectPathProvider);

            context.RegisterSourceOutput(combined, (sourceProductionContext, source) => {
                GenerateOutput(
                    sourceProductionContext
                    , source.Left.Right
                    , source.Left.Left
                    , source.Right.projectPath
                    , source.Right.outputSourceGenFiles
                );
            });
        }

        public static bool IsSyntaxMatch(
              SyntaxNode syntaxNode
            , CancellationToken token
        )
        {
            token.ThrowIfCancellationRequested();

            return false;
        }

        public static BaseTypeDeclarationSyntax GetSemanticMatch(
              GeneratorSyntaxContext syntaxContext
            , CancellationToken token
        )
        {
            return null;
        }

        private static void GenerateOutput(
              SourceProductionContext context
            , Compilation compilation
            , BaseTypeDeclarationSyntax candidate
            , string projectPath
            , bool outputSourceGenFiles
        )
        {

        }
    }
}
