using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    [Generator]
    public class InternalUnionGenerator : IIncrementalGenerator
    {
        public const string INTERFACE = "global::ZBase.Foundation.Mvvm.IObservableObject";
        public const string OBSERVABLE_PROPERTY_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.ObservablePropertyAttribute";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var projectPathProvider = SourceGenHelpers.GetSourceGenConfigProvider(context);

            var candidateProvider = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (node, token) => IsSyntaxMatch(node, token),
                transform: static (syntaxContext, token) => GetSemanticMatch(syntaxContext, token)
            ).Where(static t => t is { });

            var combined = context.CompilationProvider
                .Combine(candidateProvider.Collect())
                .Combine(projectPathProvider);

            context.RegisterSourceOutput(combined, static (sourceProductionContext, source) => {
                GenerateOutput(
                    sourceProductionContext
                    , source.Left.Left
                    , source.Left.Right
                    , source.Right.projectPath
                    , source.Right.outputSourceGenFiles
                );
            });
        }

        public static bool IsSyntaxMatch(SyntaxNode syntaxNode, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            
            return syntaxNode is FieldDeclarationSyntax field
                && field.AttributeLists.Count > 0
                && field.Parent is ClassDeclarationSyntax;
        }

        public static ClassDeclarationSyntax GetSemanticMatch(
              GeneratorSyntaxContext context
            , CancellationToken token
        )
        {
            var fieldSyntax = (FieldDeclarationSyntax)context.Node;
            var isCandidate = false;

            foreach (var attributeListSyntax in fieldSyntax.AttributeLists)
            {
                foreach (var attributeSyntax in attributeListSyntax.Attributes)
                {
                    var typeInfo = context.SemanticModel.GetSymbolInfo(attributeSyntax, token);

                    if (typeInfo.Symbol is not IMethodSymbol attributeSymbol)
                    {
                        continue;
                    }

                    var attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                    var fullName = attributeContainingTypeSymbol.ToFullName();

                    if (fullName.StartsWith(OBSERVABLE_PROPERTY_ATTRIBUTE))
                    {
                        isCandidate = true;
                        goto PROCESS_CANDIDATE;
                    }
                }
            }

            PROCESS_CANDIDATE:

            if (isCandidate == false)
            {
                return null;
            }

            if (fieldSyntax.Parent is not ClassDeclarationSyntax classSyntax)
            {
                return null;
            }

            foreach (var baseType in classSyntax.BaseList.Types)
            {
                var typeInfo = context.SemanticModel.GetTypeInfo(baseType.Type, token);

                if (typeInfo.Type.ToFullName().StartsWith(INTERFACE))
                {
                    return classSyntax;
                }

                if (typeInfo.Type.ImplementsInterface(INTERFACE))
                {
                    return classSyntax;
                }
            }

            return null;
        }

        private static void GenerateOutput(
              SourceProductionContext context
            , Compilation compilation
            , ImmutableArray<ClassDeclarationSyntax> candidates
            , string projectPath
            , bool outputSourceGenFiles
        )
        {
            if (candidates.Length < 1)
            {
                return;
            }

            context.CancellationToken.ThrowIfCancellationRequested();

            SourceGenHelpers.ProjectPath = projectPath;

            var declaration = new InternalUnionDeclaration(candidates, compilation, context.CancellationToken);

            declaration.GenerateUnionTypes(
                  context
                , compilation
                , outputSourceGenFiles
            );

            declaration.GenerateStaticClass(
                  context
                , compilation
                , outputSourceGenFiles
            );
        }

        private static readonly DiagnosticDescriptor s_errorDescriptor
            = new("SG_INTERNAL_UNION_01"
                , "Internal Union Generator Error"
                , "This error indicates a bug in the Internal Union source generators. Error message: '{0}'."
                , "ZBase.Foundation.Mvvm.ObservablePropertyAttribute"
                , DiagnosticSeverity.Error
                , isEnabledByDefault: true
                , description: ""
            );
    }
}
