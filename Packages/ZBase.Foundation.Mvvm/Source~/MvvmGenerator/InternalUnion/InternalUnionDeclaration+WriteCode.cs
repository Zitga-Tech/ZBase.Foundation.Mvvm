using Microsoft.CodeAnalysis;
using System;
using System.Collections.Generic;
using ZBase.Foundation.SourceGen;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ZBase.Foundation.Mvvm
{
    partial class InternalUnionDeclaration
    {
        private const string AGGRESSIVE_INLINING = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]";
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.InternalUnionGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";
        public const string STRUCT_LAYOUT = "[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Explicit, Size = global::ZBase.Foundation.Unions.UnionData.SIZE)]";
        public const string META_OFFSET = "[global::System.Runtime.InteropServices.FieldOffset(global::ZBase.Foundation.Unions.UnionBase.META_OFFSET)]";
        public const string DATA_OFFSET = "[global::System.Runtime.InteropServices.FieldOffset(global::ZBase.Foundation.Unions.UnionBase.DATA_OFFSET)]";
        public const string UNION_TYPE = "global::ZBase.Foundation.Unions.Union";
        public const string UNION_TYPE_KIND = "global::ZBase.Foundation.Unions.UnionTypeKind";
        public const string DOES_NOT_RETURN = "[global::System.Diagnostics.CodeAnalysis.DoesNotReturn]";
        public const string RUNTIME_INITIALIZE_ON_LOAD_METHOD = "[global::UnityEngine.RuntimeInitializeOnLoadMethod(global::UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]";

        public const string GENERATOR_NAME = nameof(InternalUnionGenerator);

        public void GenerateUnionTypes(
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
                    var source = WriteInternalUnion(typeRef, compilation.Assembly.Name);
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
                var source = WriteInternalClass(Types, assemblyName);
                var sourceFilePath = syntaxTree.GetGeneratedSourceFilePath(assemblyName, GENERATOR_NAME);

                var outputSource = TypeCreationHelpers.GenerateSourceTextForRootNodes(
                      sourceFilePath
                    , syntax
                    , source
                    , context.CancellationToken
                );

                var fileName = $"InternalUnions_{assemblyName}";

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

        /// <summary>
        /// <see cref="WriteFields(string, string, Printer)"/>
        /// </summary>
        /// <param name="types"></param>
        /// <param name="assemblyName"></param>
        /// <returns></returns>
        private static string WriteInternalClass(List<TypeRef> types, string assemblyName)
        {
            var p = Printer.DefaultLarge;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p.PrintLine($"namespace ZBase.Foundation.Mvvm.__InternalUnions.{assemblyName}");
            p.OpenScope();
            {
                p.PrintLine("/// <summary>");
                p.PrintLine("/// Contains auto-generated unions for types that are the type of either");
                p.PrintLine("/// [ObservableProperty] properties or the parameter of [RelayCommand] methods.");
                p.PrintLine("/// <br/>");
                p.PrintLine("/// These unions are not intended to be used directly by user-code");
                p.PrintLine("/// thus they are declared <c>private</c> inside this class.");
                p.PrintLine("/// </summary>");
                p.Print("#if !UNITY_5_3_OR_NEWER").PrintEndLine();
                p.PrintLine("/// <remarks>");
                p.PrintLine("/// Call the <see cref=\"Register()\"/> method to register unions inside this class");
                p.PrintLine("/// to <see cref=\"ZBase.Foundation.Unions.UnionConverter\"/>.");
                p.PrintLine("/// </remarks>");
                p.Print("#endif").PrintEndLine();
                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
                p.PrintLine("public static partial class InternalUnions");
                p.OpenScope();
                {
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
                    p.PrintLine("static InternalUnions()");
                    p.OpenScope();
                    {
                        p.PrintLine("Init();");
                    }
                    p.CloseScope();
                    p.PrintEndLine();

                    p.Print("#if !UNITY_5_3_OR_NEWER").PrintEndLine();
                    p.PrintLine("/// <summary>");
                    p.PrintLine("/// Register all unions inside this class");
                    p.PrintLine("/// to <see cref=\"ZBase.Foundation.Unions.UnionConverter\"/>");
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
                        foreach (var typeRef in types)
                        {
                            var symbol = typeRef.Symbol;
                            var typeName = symbol.ToFullName();
                            var simpleTypeName = symbol.ToSimpleName();
                            var identifier = symbol.ToValidIdentifier();
                            var converterDefault = $"Union__{identifier}.Converter.Default";

                            p.PrintLine($"global::ZBase.Foundation.Unions.UnionConverter.TryRegister<{typeName}>({converterDefault});");
                            p.PrintEndLine();

                            p.Print("#if UNITY_5_3_OR_NEWER && UNITY_EDITOR && LOG_INTERNAL_UNIONS_REGISTRATION").PrintEndLine();
                            p.PrintLine($"global::UnityEngine.Debug.Log(\"Register internal union for {simpleTypeName}\");");
                            p.Print("#endif").PrintEndLine();
                        }
                    }
                    p.CloseScope();
                    p.PrintEndLine();

                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine("[global::System.AttributeUsage(");
                    p = p.IncreasedIndent();
                    {
                        p.PrintLine("  global::System.AttributeTargets.Assembly");
                        p.PrintLine("| global::System.AttributeTargets.Class");
                        p.PrintLine("| global::System.AttributeTargets.Struct");
                        p.PrintLine("| global::System.AttributeTargets.Enum");
                        p.PrintLine("| global::System.AttributeTargets.Constructor");
                        p.PrintLine("| global::System.AttributeTargets.Method");
                        p.PrintLine("| global::System.AttributeTargets.Property");
                        p.PrintLine("| global::System.AttributeTargets.Field");
                        p.PrintLine("| global::System.AttributeTargets.Event");
                        p.PrintLine("| global::System.AttributeTargets.Interface");
                        p.PrintLine("| global::System.AttributeTargets.Delegate");
                        p.PrintLine(", Inherited = false");
                    }
                    p = p.DecreasedIndent();
                    p.PrintLine(")]");
                    p.PrintLine("public sealed class PreserveAttribute : global::System.Attribute { }");
                }
                p.CloseScope();
            }
            p.CloseScope();

            return p.Result;
        }

        private static string WriteInternalUnion(TypeRef typeRef, string assemblyName)
        {
            var symbol = typeRef.Symbol;
            var typeName = symbol.ToFullName();
            var identifier = symbol.ToValidIdentifier();
            var internalUnionName = $"Union__{identifier}";
            var unionName = $"Union<{typeName}>";

            var p = Printer.DefaultLarge;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p.PrintLine($"namespace ZBase.Foundation.Mvvm.__InternalUnions.{assemblyName}");
            p.OpenScope();
            {
                p.PrintLine("static partial class InternalUnions");
                p.OpenScope();
                {
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
                    p.PrintLine(STRUCT_LAYOUT);
                    p.PrintBeginLine()
                        .Print("private partial struct ").Print(internalUnionName)
                        .Print($" : global::ZBase.Foundation.Unions.IUnion<{typeName}>")
                        .PrintEndLine();
                    p.OpenScope();
                    {
                        p = WriteFields(typeName, unionName, p);
                        p = WriteConstructors(typeName, internalUnionName, unionName, p);
                        p = WriteValidateTypeIdMethod(typeName, unionName, p);
                        p = WirteImplicitConversions(typeName, internalUnionName, unionName, p);
                        p = WriteConverterClass(typeName, internalUnionName, unionName, p);
                    }
                    p.CloseScope();
                }
                p.CloseScope();
            }
            p.CloseScope();

            return p.Result;
        }

        private static Printer WriteFields(string typeName, string unionName, Printer p)
        {
            p.PrintLine(META_OFFSET).PrintLine(GENERATED_CODE).PrintLine("[Preserve]");
            p.PrintLine($"public readonly {unionName} Union;");
            p.PrintEndLine();

            p.PrintLine(DATA_OFFSET).PrintLine(GENERATED_CODE).PrintLine("[Preserve]");
            p.PrintLine($"public readonly {typeName} Value;");
            p.PrintEndLine();

            return p;
        }

        private static Printer WriteConstructors(string typeName, string internalUnionName, string unionName, Printer p)
        {
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
            p.PrintLine($"public {internalUnionName}({typeName} value)");
            p.OpenScope();
            {
                p.PrintLine($"this.Union = new {UNION_TYPE}({UNION_TYPE_KIND}.ValueType, {unionName}.TypeId);");
                p.PrintLine("this.Value = value;");
            }
            p.CloseScope();
            p.PrintEndLine();

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
            p.PrintLine($"public {internalUnionName}(in {unionName} union) : this()");
            p.OpenScope();
            {
                p.PrintLine("this.Union = union;");
            }
            p.CloseScope();
            p.PrintEndLine();

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
            p.PrintLine($"public {internalUnionName}(in {UNION_TYPE} union) : this()");
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
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(DOES_NOT_RETURN).PrintLine("[Preserve]");
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

        private static Printer WirteImplicitConversions(string typeName, string internalUnionName, string unionName, Printer p)
        {
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING).PrintLine("[Preserve]");
            p.PrintLine($"public static implicit operator {internalUnionName}({typeName} value) => new {internalUnionName}(value);");
            p.PrintEndLine();

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING).PrintLine("[Preserve]");
            p.PrintLine($"public static implicit operator {UNION_TYPE}(in {internalUnionName} value) => value.Union;");
            p.PrintEndLine();

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING).PrintLine("[Preserve]");
            p.PrintLine($"public static implicit operator {unionName}(in {internalUnionName} value) => value.Union;");
            p.PrintEndLine();

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING).PrintLine("[Preserve]");
            p.PrintLine($"public static implicit operator {internalUnionName}(in {unionName} value) => new {internalUnionName}(value);");
            p.PrintEndLine();
            return p;
        }

        private static Printer WriteConverterClass(string typeName, string internalUnionName, string unionName, Printer p)
        {
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
            p.PrintBeginLine()
                .Print("public sealed class Converter")
                .Print($": global::ZBase.Foundation.Unions.IUnionConverter<{typeName}>")
                .PrintEndLine();
            p.OpenScope();
            {
                p.PrintLine(GENERATED_CODE).PrintLine("[Preserve]");
                p.PrintLine("public static readonly Converter Default = new Converter();");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
                p.PrintLine("private Converter() { }");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING).PrintLine("[Preserve]");
                p.PrintLine($"public {UNION_TYPE} ToUnion({typeName} value) => new {internalUnionName}(value);");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(AGGRESSIVE_INLINING).PrintLine("[Preserve]");
                p.PrintLine($"public {unionName} ToUnionT({typeName} value) => new {internalUnionName}(value).Union;");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
                p.PrintLine($"public bool TryGetValue(in {UNION_TYPE} union, out {typeName} result)");
                p.OpenScope();
                {
                    p.PrintLine($"if (union.TypeId == {unionName}.TypeId)");
                    p.OpenScope();
                    {
                        p.PrintLine($"var temp = new {internalUnionName}(union);");
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

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
                p.PrintLine($"public bool TrySetValue(in {UNION_TYPE} union, ref {typeName} result)");
                p.OpenScope();
                {
                    p.PrintLine($"if (union.TypeId == {unionName}.TypeId)");
                    p.OpenScope();
                    {
                        p.PrintLine($"var temp = new {internalUnionName}(union);");
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
