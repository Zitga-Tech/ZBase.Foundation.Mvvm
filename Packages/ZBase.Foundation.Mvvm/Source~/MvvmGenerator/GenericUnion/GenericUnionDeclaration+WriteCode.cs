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

        public const string GENERATOR_NAME = nameof(GenericUnionDeclaration);

        public void GenerateUnionTypes(
              SourceProductionContext context
            , Compilation compilation
            , bool outputSourceGenFiles
        )
        {
            foreach (var structRef in StructRefs)
            {
                try
                {
                    var syntax = structRef.Syntax;
                    var syntaxTree = syntax.SyntaxTree;
                    var source = WriteGenericUnion(structRef);
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
                          s_errorDescriptor
                        , structRef.Syntax.GetLocation()
                        , e.ToUnityPrintableString()
                    ));
                }
            }
        }

        public void GenerateStaticClass(
              SourceProductionContext context
            , Compilation compilation
            , bool outputSourceGenFiles
        )
        {
            var syntax = CompilationUnit().NormalizeWhitespace(eol: "\n");

            try
            {
                var syntaxTree = syntax.SyntaxTree;
                var assemblyName = compilation.Assembly.Name;
                var source = WriteStaticClass(StructRefs, assemblyName);
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
                      s_errorDescriptor
                    , syntax.GetLocation()
                    , e.ToUnityPrintableString()
                ));
            }
        }

        public void GenerateRedundantTypes(
              SourceProductionContext context
            , Compilation compilation
            , bool outputSourceGenFiles
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
                          s_errorDescriptor
                        , structRef.Syntax.GetLocation()
                        , e.ToUnityPrintableString()
                    ));
                }
            }
        }

        private static string WriteStaticClass(ImmutableArray<StructRef> structs, string assemblyName)
        {
            var p = Printer.DefaultLarge;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p.PrintLine($"namespace ZBase.Foundation.Mvvm.Unions.__Generics.{assemblyName}");
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
                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
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
                        foreach (var structRef in structs)
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
                    p.CloseScope();
                    p.PrintEndLine();

                    p = GeneratorHelpers.WritePreserveAttributeClass(p);
                }
                p.CloseScope();
            }
            p.CloseScope();

            return p.Result;
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

        private static string WriteGenericUnion(StructRef structRef)
        {
            var typeName = structRef.TypeArgument.ToFullName();
            var structName = structRef.Syntax.Identifier.Text;
            var unionName = $"Union<{typeName}>";

            var scopePrinter = new SyntaxNodeScopePrinter(Printer.DefaultLarge, structRef.Syntax.Parent);
            var p = scopePrinter.printer;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p = p.IncreasedIndent();

            p.PrintLine(STRUCT_LAYOUT);
            p.PrintBeginLine()
                .Print("partial struct ").Print(structName)
                .PrintEndLine();
            p.OpenScope();
            {
                p = WriteFields(typeName, unionName, p);
                p = WriteConstructors(typeName, structName, unionName, p);
                p = WriteValidateTypeIdMethod(typeName, unionName, p);
                p = WirteImplicitConversions(typeName, structName, unionName, p);
                p = WriteConverterClass(typeName, structName, unionName, p);
            }
            p.CloseScope();

            p = p.DecreasedIndent();
            return p.Result;
        }

        private static Printer WriteFields(string typeName, string unionName, Printer p)
        {
            p.PrintLine(META_OFFSET).PrintLine(GENERATED_CODE);
            p.PrintLine($"public readonly {unionName} Union;");
            p.PrintEndLine();

            p.PrintLine(DATA_OFFSET).PrintLine(GENERATED_CODE);
            p.PrintLine($"public readonly {typeName} Value;");
            p.PrintEndLine();

            return p;
        }

        private static Printer WriteConstructors(string typeName, string structName, string unionName, Printer p)
        {
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {structName}({typeName} value)");
            p.OpenScope();
            {
                p.PrintLine($"this.Union = new {UNION_TYPE}({UNION_TYPE_KIND}.ValueType, {unionName}.TypeId);");
                p.PrintLine("this.Value = value;");
            }
            p.CloseScope();
            p.PrintEndLine();

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {structName}(in {unionName} union) : this()");
            p.OpenScope();
            {
                p.PrintLine("this.Union = union;");
            }
            p.CloseScope();
            p.PrintEndLine();

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {structName}(in {UNION_TYPE} union) : this()");
            p.OpenScope();
            {
                p.PrintLine("ValidateTypeId(union);");
                p.PrintLine("this.Union = union;");
            }
            p.CloseScope();
            p.PrintEndLine();
            return p;
        }

        private static Printer WriteValidateTypeIdMethod(string typeName, string unionName, Printer p)
        {
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(DOES_NOT_RETURN);
            p.PrintLine($"private static void ValidateTypeId(in {UNION_TYPE} union)");
            p.OpenScope();
            {
                p.PrintLine($"if (union.TypeId != {unionName}.TypeId)");
                p.OpenScope();
                {
                    p.PrintLine("throw new global::System.InvalidCastException");
                    p.OpenScope("(");
                    {
                        p.PrintLine($"$\"Cannot cast {{union.TypeId.AsType()}} to {{typeof({typeName})}}\"");
                    }
                    p.CloseScope(");");
                }
                p.CloseScope();
            }
            p.CloseScope();
            return p;
        }

        private static Printer WirteImplicitConversions(string typeName, string structName, string unionName, Printer p)
        {
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING);
            p.PrintLine($"public static implicit operator {structName}({typeName} value) => new {structName}(value);");
            p.PrintEndLine();

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING);
            p.PrintLine($"public static implicit operator {UNION_TYPE}(in {structName} value) => value.Union;");
            p.PrintEndLine();

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING);
            p.PrintLine($"public static implicit operator {unionName}(in {structName} value) => value.Union;");
            p.PrintEndLine();

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING);
            p.PrintLine($"public static implicit operator {structName}(in {unionName} value) => new {structName}(value);");
            p.PrintEndLine();
            return p;
        }

        private static Printer WriteConverterClass(string typeName, string structName, string unionName, Printer p)
        {
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintBeginLine()
                .Print("public sealed class Converter")
                .Print($": global::ZBase.Foundation.Mvvm.Unions.IUnionConverter<{typeName}>")
                .PrintEndLine();
            p.OpenScope();
            {
                p.PrintLine(GENERATED_CODE);
                p.PrintLine("public static readonly Converter Default = new Converter();");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                p.PrintLine("private Converter() { }");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING);
                p.PrintLine($"public {UNION_TYPE} ToUnion({typeName} value) => new {structName}(value);");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING);
                p.PrintLine($"public {unionName} ToUnionT({typeName} value) => new {structName}(value).Union;");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                p.PrintLine($"public bool TryGetValue(in {UNION_TYPE} union, out {typeName} result)");
                p.OpenScope();
                {
                    p.PrintLine($"if (union.TypeId == {unionName}.TypeId)");
                    p.OpenScope();
                    {
                        p.PrintLine($"var temp = new {structName}(union);");
                        p.PrintLine("result = temp.Value;");
                        p.PrintLine("return true;");
                    }
                    p.CloseScope();
                    p.PrintEndLine();

                    p.PrintLine("result = default;");
                    p.PrintLine("return false;");
                }
                p.CloseScope();
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                p.PrintLine($"public bool TrySetValue(in {UNION_TYPE} union, ref {typeName} result)");
                p.OpenScope();
                {
                    p.PrintLine($"if (union.TypeId == {unionName}.TypeId)");
                    p.OpenScope();
                    {
                        p.PrintLine($"var temp = new {structName}(union);");
                        p.PrintLine("result = temp.Value;");
                        p.PrintLine("return true;");
                    }
                    p.CloseScope();
                    p.PrintEndLine();

                    p.PrintLine("return false;");
                }
                p.CloseScope();
                p.PrintEndLine();
            }
            p.CloseScope();
            return p;
        }

        private static readonly DiagnosticDescriptor s_errorDescriptor
            = new("SG_GENERIC_UNIONS_01"
                , "Generic Union Generator Error"
                , "This error indicates a bug in the Generic Union source generators. Error message: '{0}'."
                , "ZBase.Foundation.Mvvm.Unions.IUnion<T>"
                , DiagnosticSeverity.Error
                , isEnabledByDefault: true
                , description: ""
            );
    }
}
