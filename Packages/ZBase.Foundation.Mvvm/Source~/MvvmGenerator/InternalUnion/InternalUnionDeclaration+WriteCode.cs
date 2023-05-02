using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    partial class InternalUnionDeclaration
    {
        private const string AGGRESSIVE_INLINING = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]";
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.InternalUnionGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";
        public const string GENERATOR_NAME = nameof(InternalUnionGenerator);

        public void Generate(
              SourceProductionContext context
            , Compilation compilation
            , bool outputSourceGenFiles
        )
        {
            foreach (var typeRef in Types)
            {
                try
                {
                    var syntax = typeRef.Syntax;
                    var syntaxTree = syntax.SyntaxTree;
                    var source = WriteCode(typeRef, compilation);
                    var sourceFilePath = syntaxTree.GetGeneratedSourceFilePath(compilation.Assembly.Name, GENERATOR_NAME);

                    var outputSource = TypeCreationHelpers.GenerateSourceTextForRootNodes(
                          sourceFilePath
                        , syntax
                        , source
                        , context.CancellationToken
                    );

                    context.AddSource(
                          syntaxTree.GetGeneratedSourceFileName(GENERATOR_NAME, syntax)
                        , outputSource
                    );

                    if (outputSourceGenFiles)
                    {
                        SourceGenHelpers.OutputSourceToFile(
                              context
                            , syntax.GetLocation()
                            , sourceFilePath
                            , outputSource
                        );
                    }
                }
                catch (Exception e)
                {
                    context.ReportDiagnostic(Diagnostic.Create(
                          s_errorDescriptor
                        , typeRef.Syntax.GetLocation()
                        , e.ToUnityPrintableString()
                    ));
                }
            }
        }

        private static string WriteCode(TypeRef typeRef, Compilation compilation)
        {
            var symbol = typeRef.Symbol;
            var typeName = symbol.ToFullName();
            var identifier = symbol.ToValidIdentifier();
            var unionName = $"InternalUnion__{identifier}";

            var p = Printer.DefaultLarge;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p.PrintLine("namespace ZBase.Foundation.Mvvm.InternalUnions");
            p.OpenScope();
            {
                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                p.PrintBeginLine();
                p.Print("public partial struct ").Print(unionName);
                p.Print($" : global::ZBase.Foundation.Unions.IUnion<{typeName}>");
                p.PrintEndLine();

                p.OpenScope();
                {

                }
                p.CloseScope();
            }
            p.CloseScope();

            return p.Result;
        }

        private static readonly DiagnosticDescriptor s_errorDescriptor
            = new("SG_INTERNAL_UNIONS_01"
                , "Internal Union Generator Error"
                , "This error indicates a bug in the Internal Union source generators. Error message: '{0}'."
                , "ZBase.Foundation.Mvvm.IObservableObject"
                , DiagnosticSeverity.Error
                , isEnabledByDefault: true
                , description: ""
            );
    }
}
