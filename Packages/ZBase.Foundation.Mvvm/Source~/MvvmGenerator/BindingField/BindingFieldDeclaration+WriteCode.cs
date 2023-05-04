using System;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    partial class BindingFieldDeclaration
    {
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.BindingFieldGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";
        private const string OBSOLETE_METHOD = "[global::System.Obsolete(\"This method is not intended to be use directly by user code.\")]";
        public string WriteCodeWithoutMember()
        {
            var scopePrinter = new SyntaxNodeScopePrinter(Printer.DefaultLarge, Syntax.Parent);
            var p = scopePrinter.printer;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p = p.IncreasedIndent();

            p.PrintBeginLine();
            p.Print("partial class ").Print(Syntax.Identifier.Text);
            p.PrintEndLine();

            p = p.IncreasedIndent();

            if (IsBaseBinder)
            {
                p.PrintLine(": global::ZBase.Foundation.Mvvm.ViewBinding.IBinder");
            }

            p = p.DecreasedIndent();

            p.OpenScope();
            {
                if (IsBaseBinder == false)
                {
                    var keyword = Symbol.IsSealed ? "" : "virtual ";

                    p.PrintLine("/// <inheritdoc/>");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}void StartBinding()");
                    p.OpenScope();
                    {
                        if (IsBaseBinder)
                        {
                            p.PrintLine("base.StartBinding();");
                        }
                    }
                    p.CloseScope();

                    p.PrintEndLine();

                    p.PrintLine("/// <inheritdoc/>");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}void StopBinding()");
                    p.OpenScope();
                    {
                        if (IsBaseBinder)
                        {
                            p.PrintLine("base.StopBinding();");
                        }
                    }
                    p.CloseScope();

                    p.PrintEndLine();

                    p.PrintLine("/// <inheritdoc/>");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}bool BindPropertyTo(string propertyName, string bindingMethodName)");
                    p.OpenScope();
                    {
                        if (IsBaseBinder)
                        {
                            p.PrintLine("return base.BindPropertyTo(propertyName, bindingMethodName);");
                        }
                        else
                        {
                            p.PrintLine("return false;");
                        }
                    }
                    p.CloseScope();
                }
            }
            p.CloseScope();

            p = p.DecreasedIndent();
            return p.Result;
        }

        public string WriteCode()
        {
            var scopePrinter = new SyntaxNodeScopePrinter(Printer.DefaultLarge, Syntax.Parent);
            var p = scopePrinter.printer;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p = p.IncreasedIndent();

            WriteBindingMethodInfoAttributes(ref p);

            p.PrintBeginLine();
            p.Print("partial class ").Print(Syntax.Identifier.Text);
            p.PrintEndLine();

            p = p.IncreasedIndent();

            if (IsBaseBinder)
            {
                p.PrintLine(": global::ZBase.Foundation.Mvvm.ViewBinding.IBinder");
            }

            p = p.DecreasedIndent();

            p.OpenScope();
            {
                if (NonUnionTypes.Length > 0)
                {
                    p.PrintLine(GENERATED_CODE);
                    p.PrintLine("private readonly UnionConverters _unionConverters = new UnionConverters();");
                    p.PrintEndLine();
                }

                WriteConstantFields(ref p);
                WriteBindingFields(ref p);
                WriteConverters(ref p);
                WriteListeners(ref p);
                WriteConstructor(ref p);
                WriteStartBindingMethod(ref p);
                WriteStopBindingMethod(ref p);
                WriteSetPropertyNameMethod(ref p);

                if (NonUnionTypes.Length > 0)
                {
                    WriteUnionOverloads(ref p);
                    WriteUnionConverters(ref p);
                }
            }
            p.CloseScope();

            p = p.DecreasedIndent();
            return p.Result;
        }

        private void WriteBindingMethodInfoAttributes(ref Printer p)
        {
            const string ATTRIBUTE = "[global::ZBase.Foundation.Mvvm.ViewBinding.BindingMethodInfo({0}.{1}, typeof(global::ZBase.Foundation.Mvvm.Unions.Union))]";

            var className = Syntax.Identifier.Text;

            foreach (var member in MemberRefs)
            {
                p.PrintLine(string.Format(ATTRIBUTE, className, ConstName(member)));
            }
        }

        private void WriteConstantFields(ref Printer p)
        {
            var className = Syntax.Identifier.Text;

            foreach (var member in MemberRefs)
            {
                var name = member.Member.Name;

                p.PrintLine($"/// <summary>The name of <see cref=\"{name}\"/></summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"public const string {ConstName(member)} = nameof({className}.{name});");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteBindingFields(ref Printer p)
        {
            foreach (var member in MemberRefs)
            {
                string readonlyKeyword;

                if (ReferenceUnityEngine)
                {
                    readonlyKeyword = "";
                }
                else
                {
                    readonlyKeyword = "readonly ";
                }

                string label;

                if (string.IsNullOrWhiteSpace(member.Label))
                {
                    label = ConstName(member);
                }
                else
                {
                    label = $"\"{member.Label}\"";
                }

                p.PrintLine($"/// <summary>The binding field for <see cref=\"{member.Member.Name}\"/></summary>");
                p.Print("#if UNITY_5_3_OR_NEWER").PrintEndLine();
                p.PrintLine("[global::UnityEngine.SerializeField]");
                p.Print("#endif").PrintEndLine();
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private {readonlyKeyword}global::ZBase.Foundation.Mvvm.ViewBinding.BindingField {FieldName(member)} =  new global::ZBase.Foundation.Mvvm.ViewBinding.BindingField() {{ Label = {label} }};");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteConverters(ref Printer p)
        {
            foreach (var member in MemberRefs)
            {
                string readonlyKeyword;

                if (ReferenceUnityEngine)
                {
                    readonlyKeyword = "";
                }
                else
                {
                    readonlyKeyword = "readonly ";
                }

                string label;

                if (string.IsNullOrWhiteSpace(member.Label))
                {
                    label = ConstName(member);
                }
                else
                {
                    label = $"\"{member.Label}\"";
                }

                p.PrintLine($"/// <summary>The converter for the parameter of <see cref=\"{member.Member.Name}\"/></summary>");
                p.Print("#if UNITY_5_3_OR_NEWER").PrintEndLine();
                p.PrintLine("[global::UnityEngine.SerializeReference]");
                p.Print("#endif").PrintEndLine();
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private {readonlyKeyword}global::ZBase.Foundation.Mvvm.Conversion.Converter {ConverterName(member)} = new global::ZBase.Foundation.Mvvm.Conversion.Converter() {{ Label = {label} }};");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteListeners(ref Printer p)
        {
            var className = Syntax.Identifier.Text;

            foreach (var member in MemberRefs)
            {
                p.PrintLine($"/// <summary>");
                p.PrintLine($"/// The listener that binds <see cref=\"{member.Member.Name}\"/>");
                p.PrintLine($"/// to the property chosen by {FieldName(member)}.");
                p.PrintLine($"/// </summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private readonly global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener<{className}> {ListenerName(member)};");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteConstructor(ref Printer p)
        {
            var className = Syntax.Identifier.Text;

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintBeginLine().Print($"public {className}()");

            if (IsBaseBinder)
            {
                p.Print(" : base()");
            }

            p.PrintEndLine();

            p.OpenScope();
            {
                foreach (var member in MemberRefs)
                {
                    var methodName = MethodName(member);

                    p.PrintLine($"this.{ListenerName(member)} = new global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener<{className}>(this)");
                    p.OpenScope();
                    p.PrintLine($"OnEventAction = (instance, args) => instance.{methodName}(this.{ConverterName(member)}.Convert(args.Value))");
                    p.CloseScope("};");
                    p.PrintEndLine();
                }

                p.PrintLine($"OnConstructor();");
            }
            p.CloseScope();
            p.PrintEndLine();

            p.PrintLine($"/// <summary>Executes the logic at the end of the default constructor.</summary>");
            p.PrintLine($"/// <remarks>This method is invoked at the end of the default constructor.</remarks>");
            p.PrintLine(GENERATED_CODE);
            p.PrintLine($"partial void OnConstructor();");
            p.PrintEndLine();
        }

        private void WriteStartBindingMethod(ref Printer p)
        {
            var keyword = IsBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}void StartBinding()");
            p.OpenScope();
            {
                if (IsBaseBinder)
                {
                    p.PrintLine("base.StartBinding();");
                    p.PrintEndLine();
                }

                p.PrintLine("if (this.DataContext.ViewModel is not global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanged inpc) return;");
                p.PrintEndLine();

                foreach (var member in MemberRefs)
                {
                    p.PrintLine($"inpc.PropertyChanged(this.{FieldName(member)}.PropertyName, this.{ListenerName(member)});");
                }
            }
            p.CloseScope();

            p.PrintEndLine();
        }

        private void WriteStopBindingMethod(ref Printer p)
        {
            var keyword = IsBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}void StopBinding()");
            p.OpenScope();
            {
                if (IsBaseBinder)
                {
                    p.PrintLine("base.StopBinding();");
                    p.PrintEndLine();
                }

                foreach (var member in MemberRefs)
                {
                    p.PrintLine($"this.{ListenerName(member)}.Detach();");
                }
            }
            p.CloseScope();

            p.PrintEndLine();
        }

        private void WriteSetPropertyNameMethod(ref Printer p)
        {
            var keyword = IsBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}bool SetPropertyName(string bindingField, string propertyName)");
            p.OpenScope();
            {
                if (IsBaseBinder)
                {
                    p.PrintLine("if (base.SetPropertyName(bindingField, propertyName)) return true;");
                    p.PrintEndLine();
                }

                p.PrintLine("switch (bindingField)");
                p.OpenScope();
                {
                    foreach (var member in MemberRefs)
                    {
                        p.PrintLine($"case {ConstName(member)}:");
                        p.OpenScope();
                        {
                            p.PrintLine($"this.{FieldName(member)}.PropertyName = propertyName;");
                            p.PrintLine("return true;");
                        }
                        p.CloseScope();
                        p.PrintEndLine();
                    }
                }
                p.CloseScope();
                p.PrintEndLine();

                p.PrintLine("return false;");
            }
            p.CloseScope();

            p.PrintEndLine();
        }

        private void WriteUnionOverloads(ref Printer p)
        {
            foreach (var member in MemberRefs)
            {
                if (member.NonUnionArgumentType == null)
                {
                    continue;
                }

                var origName = member.Member.Name;
                var typeName = member.NonUnionArgumentType.ToFullName();
                var methodName = MethodName(member);
                var converter = GeneratorHelpers.ToTitleCase(member.NonUnionArgumentType.ToValidIdentifier().AsSpan());

                p.PrintLine($"/// <summary>");
                p.PrintLine($"/// This overload will try to get the value of type <see cref=\"{typeName}\"/>");
                p.PrintLine($"/// from <see cref=\"global::ZBase.Foundation.Mvvm.Unions.Union\"/>");
                p.PrintLine($"/// to pass into <see cref=\"{origName}\"/>.");
                p.PrintLine($"/// </summary>");
                p.PrintLine($"/// <remarks>This method is not intended to be use directly by user code.</remarks>");
                p.PrintLine(OBSOLETE_METHOD).PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                p.PrintLine($"private void {methodName}(in global::ZBase.Foundation.Mvvm.Unions.Union union)");
                p.OpenScope();
                {
                    p.PrintLine($"if (this._unionConverters.{converter}.TryGetValue(union, out {typeName} value))");
                    p.OpenScope();
                    {
                        p.PrintLine($"{origName}(value);");
                    }
                    p.CloseScope();
                }
                p.CloseScope();
            }

            p.PrintEndLine();
        }

        private void WriteUnionConverters(ref Printer p)
        {
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine("private class UnionConverters");
            p.OpenScope();
            {
                foreach (var type in NonUnionTypes)
                {
                    var typeName = type.ToFullName();
                    var fieldName = type.ToValidIdentifier();

                    p.PrintLine(GENERATED_CODE);
                    p.PrintLine($"private global::ZBase.Foundation.Mvvm.Unions.IUnionConverter<{typeName}> _{fieldName};");
                    p.PrintEndLine();
                }

                foreach (var type in NonUnionTypes)
                {
                    var typeName = type.ToFullName();
                    var fieldName = type.ToValidIdentifier();
                    var propertyName = GeneratorHelpers.ToTitleCase(fieldName.AsSpan());

                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public global::ZBase.Foundation.Mvvm.Unions.IUnionConverter<{typeName}> {propertyName}");
                    p.OpenScope();
                    {
                        p.PrintLine($"get => this._{fieldName} ??= global::ZBase.Foundation.Mvvm.Unions.UnionConverter.GetConverter<{typeName}>();");
                    }
                    p.CloseScope();
                    p.PrintEndLine();
                }
            }
            p.CloseScope();
            p.PrintEndLine();
        }

        private string ConstName(MemberRef member)
            => $"BindingField_{member.Member.Name}";

        private string FieldName(MemberRef member)
            => $"_field{member.Member.Name}";

        private string ConverterName(MemberRef member)
            => $"_converter{member.Member.Name}";

        private string ListenerName(MemberRef member)
            => $"_listener{member.Member.Name}";

        private string MethodName(MemberRef member)
        {
            var name = member.Member.Name;

            if (member.NonUnionArgumentType != null)
            {
                return $"{name}__Union";
            }

            return name;
        }
    }
}