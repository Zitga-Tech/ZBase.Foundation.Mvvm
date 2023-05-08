using Microsoft.CodeAnalysis;
using System;
using System.Collections.Immutable;
using ZBase.Foundation.SourceGen;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace ZBase.Foundation.Mvvm.InternalUnionSourceGen
{
    partial class InternalUnionDeclaration
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
        public const string GENERATED_INTERNAL_UNIONS = "[global::ZBase.Foundation.Mvvm.Unions.GeneratedInternalUnions]";

        public const string GENERATOR_NAME = nameof(InternalUnionGenerator);

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
                var source = GetSourceForInternalClass(ValueTypeRefs, RefTypeRefs, assemblyName);
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
                      errorDescriptor
                    , syntax.GetLocation()
                    , e.ToUnityPrintableString()
                ));
            }
        }

        private static string GetSourceForInternalClass(
              ImmutableArray<TypeRef> valueTypes
            , ImmutableArray<TypeRef> refTypes
            , string assemblyName
        )
        {
            var p = Printer.DefaultLarge;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p.PrintLine($"namespace ZBase.Foundation.Mvvm.Unions.__Internal.{assemblyName.ToValidIdentifier()}");
            p.OpenScope();
            {
                p.PrintLine("/// <summary>");
                p.PrintLine("/// Contains auto-generated unions for types that are the type of either");
                p.PrintLine("/// [ObservableProperty] properties or the parameter of [RelayCommand] methods.");
                p.PrintLine("/// <br/>");
                p.PrintLine("/// Automatically register these unions");
                p.PrintLine("/// to <see cref=\"ZBase.Foundation.Mvvm.Unions.UnionConverter\"/>");
                p.PrintLine("/// on Unity3D platform.");
                p.PrintLine("/// <br/>");
                p.PrintLine("/// These unions are not intended to be used directly by user-code");
                p.PrintLine("/// thus they are declared <c>private</c> inside this class.");
                p.PrintLine("/// </summary>");
                p.Print("#if !UNITY_5_3_OR_NEWER").PrintEndLine();
                p.PrintLine("/// <remarks>");
                p.PrintLine("/// Call the <see cref=\"Register()\"/> method to register unions inside this class");
                p.PrintLine("/// to <see cref=\"ZBase.Foundation.Mvvm.Unions.UnionConverter\"/>.");
                p.PrintLine("/// </remarks>");
                p.Print("#endif").PrintEndLine();
                p.PrintLine(GENERATED_INTERNAL_UNIONS).PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine("[Preserve]");
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
                        foreach (var typeRef in valueTypes)
                        {
                            WriteTryRegister(ref p, typeRef);
                        }

                        p.PrintEndLine();

                        foreach (var typeRef in refTypes)
                        {
                            WriteTryRegister(ref p, typeRef);
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

            static void WriteTryRegister(ref Printer p, TypeRef typeRef)
            {
                var symbol = typeRef.Symbol;
                var typeName = symbol.ToFullName();
                var simpleTypeName = symbol.ToSimpleName();
                var identifier = symbol.ToValidIdentifier();
                var converterDefault = $"Union__{identifier}.Converter.Default";

                p.PrintLine($"global::ZBase.Foundation.Mvvm.Unions.UnionConverter.TryRegister<{typeName}>({converterDefault});");
                p.PrintEndLine();

                p.Print("#if UNITY_5_3_OR_NEWER && UNITY_EDITOR && LOG_INTERNAL_UNIONS_REGISTRATION").PrintEndLine();
                p.PrintLine($"global::UnityEngine.Debug.Log(\"Register internal union for {simpleTypeName}\");");
                p.Print("#endif").PrintEndLine();
            }
        }
    }
}
