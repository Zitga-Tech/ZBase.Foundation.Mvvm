using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    [Generator]
    public class BindingFieldGenerator : IIncrementalGenerator
    {
        public const string INTERFACE = "global::ZBase.Foundation.Mvvm.IBinder";
        public const string GENERATOR_NAME = nameof(BindingFieldGenerator);

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var projectPathProvider = SourceGenHelpers.GetSourceGenConfigProvider(context);

            var candidateProvider = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (node, token) => GeneratorHelpers.IsSyntaxMatch(node, token),
                transform: static (syntaxContext, token) => GeneratorHelpers.GetSemanticMatch(syntaxContext, token, INTERFACE)
            ).Where(static t => t is { });

            var compilationProvider = context.CompilationProvider;
            var combined = candidateProvider.Combine(compilationProvider).Combine(projectPathProvider);

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

        private static void GenerateOutput(
              SourceProductionContext context
            , Compilation compilation
            , ClassDeclarationSyntax candidate
            , string projectPath
            , bool outputSourceGenFiles
        )
        {
            if (candidate == null)
            {
                return;
            }

            context.CancellationToken.ThrowIfCancellationRequested();

            try
            {
                SourceGenHelpers.ProjectPath = projectPath;

                var syntaxTree = candidate.SyntaxTree;
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var declaration = new BindingFieldDeclaration(candidate, semanticModel, context.CancellationToken);

                string source;

                if (declaration.Members.Count > 0)
                {
                    source = declaration.WriteCode();
                }
                else
                {
                    source = declaration.WriteCodeWithoutMember();
                }

                var sourceFilePath = syntaxTree.GetGeneratedSourceFilePath(compilation.Assembly.Name, GENERATOR_NAME);
                var outputSource = TypeCreationHelpers.GenerateSourceTextForRootNodes(
                      sourceFilePath
                    , candidate
                    , source
                    , context.CancellationToken
                );

                context.AddSource(
                      syntaxTree.GetGeneratedSourceFileName(GENERATOR_NAME, candidate)
                    , outputSource
                );

                if (outputSourceGenFiles)
                {
                    SourceGenHelpers.OutputSourceToFile(
                          context
                        , candidate.GetLocation()
                        , sourceFilePath
                        , outputSource
                    );
                }
            }
            catch (Exception e)
            {
                if (e is OperationCanceledException)
                {
                    throw;
                }

                context.ReportDiagnostic(Diagnostic.Create(
                      s_errorDescriptor
                    , candidate.GetLocation()
                    , e.ToUnityPrintableString()
                ));
            }
        }

        private static readonly DiagnosticDescriptor s_errorDescriptor
            = new("SG_BINDING_FIELD_01"
                , "Binding Field Generator Error"
                , "This error indicates a bug in the Binding Field source generators. Error message: '{0}'."
                , "ZBase.Foundation.Mvvm.BindingAttribute"
                , DiagnosticSeverity.Error
                , isEnabledByDefault: true
                , description: ""
            );
    }
}