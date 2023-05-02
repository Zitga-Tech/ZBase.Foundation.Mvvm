using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Linq;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    [Generator]
    public class RelayCommandGenerator : IIncrementalGenerator
    {
        public const string RELAY_COMMAND = "RelayCommand";
        public const string GENERATOR_NAME = nameof(RelayCommandGenerator);

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var projectPathProvider = SourceGenHelpers.GetSourceGenConfigProvider(context);

            var candidateProvider = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: (node, token) => GeneratorHelpers.IsSyntaxMatchByAttribute(node, token, SyntaxKind.MethodDeclaration, RELAY_COMMAND),
                transform: GeneratorHelpers.GetSemanticMatch
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
                var declaration = new RelayCommandDeclaration(candidate, semanticModel, context.CancellationToken);

                if (declaration.IsValid == false)
                {
                    return;
                }

                var source = declaration.WriteCode();
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
            = new("SG_RELAY_COMMAND_01"
                , "Relay Command Generator Error"
                , "This error indicates a bug in the RelayCommand source generators. Error message: '{0}'."
                , "ZBase.Foundation.Mvvm.IObservableObject"
                , DiagnosticSeverity.Error
                , isEnabledByDefault: true
                , description: ""
            );
    }
}