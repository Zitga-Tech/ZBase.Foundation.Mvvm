using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    partial class BindingFieldDeclaration
    {
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.BindingFieldGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";

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
                p.PrintLine(": global::ZBase.Foundation.Mvvm.IBinder");
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

            WriteBindingInfoAttributes(ref p);

            p.PrintBeginLine();
            p.Print("partial class ").Print(Syntax.Identifier.Text);
            p.PrintEndLine();

            p = p.IncreasedIndent();

            if (IsBaseBinder)
            {
                p.PrintLine(": global::ZBase.Foundation.Mvvm.IBinder");
            }

            p = p.DecreasedIndent();

            p.OpenScope();
            {
                WriteConstantFields(ref p);
                WriteBindingFields(ref p);
                WriteConverters(ref p);
                WriteListeners(ref p);
                WriteConstructor(ref p);
                WriteStartBindingMethod(ref p);
                WriteStopBindingMethod(ref p);
                WriteBindPropertyToMethod(ref p);
            }
            p.CloseScope();

            p = p.DecreasedIndent();
            return p.Result;
        }

        private void WriteBindingInfoAttributes(ref Printer p)
        {
            const string ATTRIBUTE = "[global::ZBase.Foundation.Mvvm.BindingInfoAttribute({0}.{1}, typeof(global::ZBase.Foundation.Mvvm.Unions.Union))]";

            var className = Syntax.Identifier.Text;

            foreach (var member in Members)
            {
                p.PrintLine(string.Format(ATTRIBUTE, className, ConstName(member)));
            }
        }

        private void WriteConstantFields(ref Printer p)
        {
            var className = Syntax.Identifier.Text;

            foreach (var member in Members)
            {
                var name = member.Member.Name;

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"public const string {ConstName(member)} = nameof({className}.{name});");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteBindingFields(ref Printer p)
        {
            foreach (var member in Members)
            {
                string readonlyKeyword;

                if (ReferenceUnityEngine)
                {
                    readonlyKeyword = "";
                    p.PrintLine("[global::UnityEngine.SerializeField]");
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

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private {readonlyKeyword}global::ZBase.Foundation.Mvvm.BindingField {FieldName(member)} =  new global::ZBase.Foundation.Mvvm.BindingField() {{ Label = {label} }};");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteConverters(ref Printer p)
        {
            foreach (var member in Members)
            {
                string readonlyKeyword;

                if (ReferenceUnityEngine)
                {
                    readonlyKeyword = "";
                    p.PrintLine("[global::UnityEngine.SerializeReference]");
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

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private {readonlyKeyword}global::ZBase.Foundation.Mvvm.Converter {ConverterName(member)} = new global::ZBase.Foundation.Mvvm.Converter() {{ Label = {label} }};");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteListeners(ref Printer p)
        {
            var className = Syntax.Identifier.Text;

            foreach (var member in Members)
            {
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private readonly global::ZBase.Foundation.Mvvm.PropertyChangeEventListener<{className}> {ListenerName(member)};");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteConstructor(ref Printer p)
        {
            var className = Syntax.Identifier.Text;

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {className}()");
            p.OpenScope();
            {
                foreach (var member in Members)
                {
                    var methodName = member.Member.Name;

                    p.PrintLine($"this.{ListenerName(member)} = new global::ZBase.Foundation.Mvvm.PropertyChangeEventListener<{className}>(this)");
                    p.OpenScope();
                    p.PrintLine($"OnEventAction = (instance, args) => instance.{methodName}(this.{ConverterName(member)}.Convert(args.Value))");
                    p.CloseScope("};");
                    p.PrintEndLine();
                }

                p.PrintLine($"OnConstruct{className}();");
            }
            p.CloseScope();
            p.PrintEndLine();

            p.PrintLine($"partial void OnConstruct{className}();");
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

                p.PrintLine("if (this.DataContext.ViewModel is not global::ZBase.Foundation.Mvvm.INotifyPropertyChanged inpc) return;");
                p.PrintEndLine();

                foreach (var member in Members)
                {
                    p.PrintLine($"inpc.PropertyChanged(this.{FieldName(member)}.ObservablePropertyName, this.{ListenerName(member)});");
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

                foreach (var member in Members)
                {
                    p.PrintLine($"this.{ListenerName(member)}.Detach();");
                }
            }
            p.CloseScope();

            p.PrintEndLine();
        }

        private void WriteBindPropertyToMethod(ref Printer p)
        {
            var keyword = IsBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}bool BindPropertyTo(string propertyName, string bindingMethodName)");
            p.OpenScope();
            {
                if (IsBaseBinder)
                {
                    p.PrintLine("if (base.BindPropertyTo(propertyName, bindingMethodName)) return true;");
                    p.PrintEndLine();
                }

                p.PrintLine("if (this.DataContext.ViewModel is not global::ZBase.Foundation.Mvvm.INotifyPropertyChanged inpc) return false;");
                p.PrintEndLine();

                p.PrintLine("switch (bindingMethodName)");
                p.OpenScope();
                {
                    foreach (var member in Members)
                    {
                        p.PrintLine($"case {ConstName(member)}:");
                        p.OpenScope();
                        {
                            p.PrintLine($"this.{ListenerName(member)}.Detach();");
                            p.PrintLine($"this.{FieldName(member)}.ObservablePropertyName = propertyName;");
                            p.PrintLine($"inpc.PropertyChanged(this.{FieldName(member)}.ObservablePropertyName, this.{ListenerName(member)});");
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

        private string ConstName(MemberRef member)
            => $"Binding_{member.Member.Name}";

        private string FieldName(MemberRef member)
            => $"_field{member.Member.Name}";

        private string ConverterName(MemberRef member)
            => $"_converter{member.Member.Name}";

        private string ListenerName(MemberRef member)
            => $"_listener{member.Member.Name}";
    }
}