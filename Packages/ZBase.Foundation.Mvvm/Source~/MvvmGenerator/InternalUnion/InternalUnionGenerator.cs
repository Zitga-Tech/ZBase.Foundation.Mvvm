using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm.InternalUnionSourceGen
{
    [Generator]
    public class InternalUnionGenerator : IIncrementalGenerator
    {
        private const string IOBSERVABLE_OBJECT = "global::ZBase.Foundation.Mvvm.ComponentModel.IObservableObject";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var projectPathProvider = SourceGenHelpers.GetSourceGenConfigProvider(context);

            var candidateProvider = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (node, token) => IsSyntaxMatch(node, token),
                transform: static (syntaxContext, token) => GetSemanticMatch(syntaxContext, token)
            ).Where(static t => t is { });

            var combined = candidateProvider.Collect()
                .Combine(context.CompilationProvider)
                .Combine(projectPathProvider);

            context.RegisterSourceOutput(combined, static (sourceProductionContext, source) => {
                GenerateOutput(
                    sourceProductionContext
                    , source.Left.Right
                    , source.Left.Left
                    , source.Right.projectPath
                    , source.Right.outputSourceGenFiles
                );
            });
        }

        public static bool IsSyntaxMatch(SyntaxNode syntaxNode, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            
            if (syntaxNode is FieldDeclarationSyntax field)
            {
                if (field.HasAttributeCandidate("ZBase.Foundation.Mvvm.ComponentModel", "ObservableProperty"))
                {
                    return true;
                }
            }

            if (syntaxNode is MethodDeclarationSyntax method && method.ParameterList.Parameters.Count == 1)
            {
                if (method.HasAttributeCandidate("ZBase.Foundation.Mvvm.Input", "RelayCommand")
                    || method.HasAttributeCandidate("ZBase.Foundation.Mvvm.ViewBinding", "Binding")
                )
                {
                    return true;
                }
            }

            if (syntaxNode is PropertyDeclarationSyntax property
                && property.Parent is ClassDeclarationSyntax classSyntax
                && classSyntax.BaseList.Types.Count > 0
            )
            {
                return true;
            }

            return false;
        }

        public static TypeRef GetSemanticMatch(
              GeneratorSyntaxContext context
            , CancellationToken token
        )
        {
            if (context.SemanticModel.Compilation.IsValidCompilation() == false)
            {
                return null;
            }

            var semanticModel = context.SemanticModel;

            if (context.Node is FieldDeclarationSyntax field)
            {
                return new TypeRef {
                    Syntax = field.Declaration.Type,
                    Symbol = semanticModel.GetTypeInfo(field.Declaration.Type).Type,
                };
            }

            if (context.Node is MethodDeclarationSyntax method)
            {
                var typeSyntax = method.ParameterList.Parameters[0].Type;

                return new TypeRef {
                    Syntax = typeSyntax,
                    Symbol = semanticModel.GetTypeInfo(typeSyntax).Type,
                };
            }

            if (context.Node is PropertyDeclarationSyntax property
                && property.Parent is ClassDeclarationSyntax classSyntax
                && classSyntax.DoesSemanticMatch(IOBSERVABLE_OBJECT, context.SemanticModel, token)
            )
            {
                return new TypeRef {
                    Syntax = property.Type,
                    Symbol = semanticModel.GetTypeInfo(property.Type).Type,
                };
            }

            return null;
        }

        private static void GenerateOutput(
              SourceProductionContext context
            , Compilation compilation
            , ImmutableArray<TypeRef> candidates
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

            var declaration = new InternalUnionDeclaration(candidates);

            declaration.GenerateUnionForValueTypes(
                  context
                , compilation
                , outputSourceGenFiles
            );

            declaration.GenerateUnionForRefTypes(
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
    }
}
