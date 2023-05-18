using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;
using ZBase.Foundation.SourceGen;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ZBase.Foundation.Mvvm.GenericUnionSourceGen
{
    partial class GenericUnionDeclaration
    {
        private const string AGGRESSIVE_INLINING = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]";
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.InternalUnionGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";
        public const string STRUCT_LAYOUT = "[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Explicit, Size = global::ZBase.Foundation.Mvvm.Unions.UnionData.SIZE)]";
        public const string META_OFFSET = "[global::System.Runtime.InteropServices.FieldOffset(global::ZBase.Foundation.Mvvm.Unions.UnionBase.META_OFFSET)]";
        public const string DATA_OFFSET = "[global::System.Runtime.InteropServices.FieldOffset(global::ZBase.Foundation.Mvvm.Unions.UnionBase.DATA_OFFSET)]";
        public const string UNION_TYPE = "global::ZBase.Foundation.Mvvm.Unions.Union";
        public const string UNION_TYPE_KIND = "global::ZBase.Foundation.Mvvm.Unions.UnionTypeKind";
        public const string DOES_NOT_RETURN = "[global::System.Diagnostics.CodeAnalysis.DoesNotReturn]";
        public const string RUNTIME_INITIALIZE_ON_LOAD_METHOD = "[global::UnityEngine.RuntimeInitializeOnLoadMethod(global::UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]";
        public const string GENERATED_GENERIC_UNIONS = "[global::ZBase.Foundation.Mvvm.Unions.SourceGen.GeneratedGenericUnions]";

        public const string GENERATOR_NAME = nameof(GenericUnionDeclaration);

        public void GenerateStaticClass(
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
                var source = WriteStaticClass(ValueTypeRefs, RefTypeRefs, assemblyName);
                var sourceFilePath = syntaxTree.GetGeneratedSourceFilePath(assemblyName, GENERATOR_NAME);

                var outputSource = TypeCreationHelpers.GenerateSourceTextForRootNodes(
                      sourceFilePath
                    , syntax
                    , source
                    , context.CancellationToken
                );

                var fileName = $"GenericUnions_{assemblyName}";

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

        public void GenerateRedundantTypes(
              SourceProductionContext context
            , Compilation compilation
            , bool outputSourceGenFiles
            , DiagnosticDescriptor errorDescriptor
        )
        {
            foreach (var structRef in Redundants)
            {
                try
                {
                    var syntax = structRef.Syntax;
                    var syntaxTree = syntax.SyntaxTree;
                    var source = WriteRedundantType(structRef);
                    var sourceFilePath = syntaxTree.GetGeneratedSourceFilePath(compilation.Assembly.Name, GENERATOR_NAME);

                    var outputSource = TypeCreationHelpers.GenerateSourceTextForRootNodes(
                          sourceFilePath
                        , syntax
                        , source
                        , context.CancellationToken
                    );

                    context.AddSource(
                          syntaxTree.GetGeneratedSourceFileName(GENERATOR_NAME, syntax, structRef.Symbol.ToValidIdentifier())
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
                        , structRef.Syntax.GetLocation()
                        , e.ToUnityPrintableString()
                    ));
                }
            }
        }

        private static string WriteStaticClass(
              ImmutableArray<StructRef> valueTypes
            , ImmutableArray<StructRef> refTypes
            , string assemblyName
        )
        {
            var p = Printer.DefaultLarge;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p.PrintLine($"namespace ZBase.Foundation.Mvvm.Unions.__Generics.{assemblyName.ToValidIdentifier()}");
            p.OpenScope();
            {
                p.PrintLine("/// <summary>");
                p.PrintLine("/// Automatically registers generic unions");
                p.PrintLine("/// to <see cref=\"ZBase.Foundation.Mvvm.Unions.UnionConverter\"/>");
                p.PrintLine("/// on Unity3D platform.");
                p.PrintLine("/// </summary>");
                p.Print("#if !UNITY_5_3_OR_NEWER").PrintEndLine();
                p.PrintLine("/// <remarks>");
                p.PrintLine("/// Call the <see cref=\"Register()\"/> method to register generic unions");
                p.PrintLine("/// to <see cref=\"ZBase.Foundation.Mvvm.Unions.UnionConverter\"/>.");
                p.PrintLine("/// </remarks>");
                p.Print("#endif").PrintEndLine();
                p.PrintLine(GENERATED_GENERIC_UNIONS).PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
                p.PrintLine("public static partial class GenericUnions");
                p.OpenScope();
                {
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
                    p.PrintLine("static GenericUnions()");
                    p.OpenScope();
                    {
                        p.PrintLine("Init();");
                    }
                    p.CloseScope();
                    p.PrintEndLine();

                    p.Print("#if !UNITY_5_3_OR_NEWER").PrintEndLine();
                    p.PrintLine("/// <summary>");
                    p.PrintLine("/// Register all generic unions");
                    p.PrintLine("/// to <see cref=\"ZBase.Foundation.Mvvm.Unions.UnionConverter\"/>");
                    p.PrintLine("/// </summary>");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
                    p.PrintLine("public static void Register() { }");
                    p.Print("#endif").PrintEndLine();
                    p.PrintEndLine();

                    p.Print("#if UNITY_5_3_OR_NEWER").PrintEndLine();
                    p.PrintLine(RUNTIME_INITIALIZE_ON_LOAD_METHOD);
                    p.Print("#endif").PrintEndLine();
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
                    p.PrintLine("private static void Init()");
                    p.OpenScope();
                    {
                        foreach (var structRef in valueTypes)
                        {
                            WriteTryRegister(ref p, structRef);
                        }

                        p.PrintEndLine();

                        foreach (var structRef in refTypes)
                        {
                            WriteTryRegister(ref p, structRef);
                        }
                    }
                    p.CloseScope();
                    p.PrintEndLine();

                    p = GeneratorHelpers.WritePreserveAttributeClass(p);
                }
                p.CloseScope();
            }
            p.CloseScope();

            return p.Result;

            #region METHODS
            #endregion ====

            static void WriteTryRegister(ref Printer p, StructRef structRef)
            {
                var symbol = structRef.Symbol;
                var typeName = structRef.TypeArgument.ToFullName();
                var simpleTypeName = structRef.TypeArgument.ToSimpleName();
                var identifier = symbol.ToFullName();
                var converterDefault = $"{identifier}.Converter.Default";

                p.PrintLine($"global::ZBase.Foundation.Mvvm.Unions.UnionConverter.TryRegister<{typeName}>({converterDefault});");
                p.PrintEndLine();

                p.Print("#if UNITY_5_3_OR_NEWER && UNITY_EDITOR && LOG_GENERIC_UNIONS_REGISTRATION").PrintEndLine();
                p.PrintLine($"global::UnityEngine.Debug.Log(\"Register generic union for {simpleTypeName}\");");
                p.Print("#endif").PrintEndLine();
            }
        }

        private static string WriteRedundantType(StructRef structRef)
        {
            var typeName = structRef.TypeArgument.ToFullName();
            var structName = structRef.Syntax.Identifier.Text;

            var scopePrinter = new SyntaxNodeScopePrinter(Printer.DefaultLarge, structRef.Syntax.Parent);
            var p = scopePrinter.printer;

            p = p.IncreasedIndent();

            p.PrintLine("/// <summary>");
            p.PrintLine($"/// A union has already been implemented for <see cref=\"{typeName}\"/.");
            p.PrintLine("/// This declaration is redundant and can be removed.");
            p.PrintLine("/// </summary>");
            p.PrintLine($"[global::System.Obsolete(\"A union has already been implemented for {typeName}. This declaration is redundant and can be removed.\")]");
            p.PrintLine($"partial struct {structName} {{ }}");

            p = p.DecreasedIndent();
            return p.Result;
        }
    }
}
