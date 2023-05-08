using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;
using ZBase.Foundation.SourceGen;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ZBase.Foundation.Mvvm.InternalStringAdapterSourceGen
{
    partial class InternalStringAdapterDeclaration
    {
        private const string AGGRESSIVE_INLINING = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]";
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.InternalUnionGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";
        private const string IADAPTER = "global::ZBase.Foundation.Mvvm.ViewBinding.IAdapter";
        private const string ADAPTER_ATTRIBUTE = "[global::ZBase.Foundation.Mvvm.ViewBinding.Adapter(fromType: typeof({0}), toType: typeof(string), order: 1)]";
        private const string LABEL_ATTRIBUTE = "[global::ZBase.Foundation.Mvvm.Label(\"{0}\", \"{1}\")]";
        private const string UNION = "global::ZBase.Foundation.Mvvm.Unions.Union";
        private const string GENERATOR_NAME = nameof(InternalStringAdapterGenerator);

        public void GenerateAdapters(
              SourceProductionContext context
            , Compilation compilation
            , bool outputSourceGenFiles
            , DiagnosticDescriptor errorDescriptor
        )
        {
            var syntax = CompilationUnit().NormalizeWhitespace(eol: "\n");

            try
            {
                var syntaxTree = syntax.SyntaxTree;
                var assemblyName = compilation.Assembly.Name;
                var source = WriteAdapter(TypeRefs, assemblyName);
                var sourceFilePath = syntaxTree.GetGeneratedSourceFilePath(assemblyName, GENERATOR_NAME);

                var outputSource = TypeCreationHelpers.GenerateSourceTextForRootNodes(
                      sourceFilePath
                    , syntax
                    , source
                    , context.CancellationToken
                );

                var fileName = $"InternalStringAdapters_{assemblyName}";

                context.AddSource(
                      syntaxTree.GetGeneratedSourceFileName(GENERATOR_NAME, fileName, syntax)
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
                      errorDescriptor
                    , syntax.GetLocation()
                    , e.ToUnityPrintableString()
                ));
            }
        }

        private static string WriteAdapter(ImmutableArray<TypeRef> types, string assemblyName)
        {
            var p = Printer.DefaultLarge;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p.PrintLine($"namespace ZBase.Foundation.Mvvm.ViewBinding.__InternalStringAdapters.{assemblyName.ToValidIdentifier()}");
            p.OpenScope();
            {
                foreach (var type in types)
                {
                    var adapterTypeName = AdapterTypeName(type);
                    var typeName = type.Symbol.ToFullName();
                    var label = $"{type.Symbol.Name} ⇒ String";
                    
                    p.PrintLine(string.Format(ADAPTER_ATTRIBUTE, typeName));
                    p.PrintLine(string.Format(LABEL_ATTRIBUTE, label, $"Generated/{type.Symbol.ContainingNamespace}"));
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public sealed class {adapterTypeName} : {IADAPTER}");
                    p.OpenScope();
                    {
                        p.PrintLine(AGGRESSIVE_INLINING).PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                        p.PrintLine($"public {UNION} Convert(in {UNION} union)");
                        p.OpenScope();
                        {
                            p.PrintLine($"return {UNION}<{typeName}>.GetConverter().ToString(union);");
                        }
                        p.CloseScope();
                    }
                    p.CloseScope();
                }
            }
            p.CloseScope();

            return p.Result;
        }

        private static string AdapterTypeName(TypeRef typeRef)
            => $"{typeRef.Symbol.ToValidIdentifier()}ToStringAdapter";
    }
}
