using System;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm.BinderSourceGen
{
    partial class BinderDeclaration
    {
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.BinderGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";
        private const string OBSOLETE_METHOD = "[global::System.Obsolete(\"This method is not intended to be use directly by user code.\")]";
        private const string GENERATED_BINDING_PROPERTY = "[global::ZBase.Foundation.Mvvm.ViewBinding.GeneratedBindingProperty({0}, typeof({1}))]";
        private const string GENERATED_CONVERTER = "[global::ZBase.Foundation.Mvvm.ViewBinding.GeneratedConverter({0}, typeof({1}))]";

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
                    p.PrintLine($"public {keyword}void StartListening()");
                    p.OpenScope();
                    {
                        if (IsBaseBinder)
                        {
                            p.PrintLine("base.StartListening();");
                        }
                    }
                    p.CloseScope();

                    p.PrintEndLine();

                    p.PrintLine("/// <inheritdoc/>");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}void StopListening()");
                    p.OpenScope();
                    {
                        if (IsBaseBinder)
                        {
                            p.PrintLine("base.StopListening();");
                        }
                    }
                    p.CloseScope();

                    p.PrintEndLine();

                    p.PrintLine("/// <inheritdoc/>");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}bool SetTargetPropertyName(string bindingPropertyName, string targetPropertyName)");
                    p.OpenScope();
                    {
                        if (IsBaseBinder)
                        {
                            p.PrintLine("return base.SetTargetPropertyName(bindingPropertyName, targetPropertyName);");
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
                WriteBindingProperties(ref p);
                WriteConverters(ref p);
                WriteListeners(ref p);
                WriteFlags(ref p);
                WriteConstructor(ref p);
                WriteStartListeningMethod(ref p);
                WriteStopListeningMethod(ref p);
                WriteSetTargetPropertyNameMethod(ref p);

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

            var className = Symbol.ToFullName();

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

        private void WriteBindingProperties(ref Printer p)
        {
            foreach (var member in MemberRefs)
            {
                if (member.SkipBindingProperty)
                {
                    continue;
                }

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

                if (string.IsNullOrWhiteSpace(member.BindingPropertyLabel))
                {
                    label = ConstName(member);
                }
                else
                {
                    label = $"\"{member.BindingPropertyLabel}\"";
                }

                var typeName = member.NonUnionArgumentType.ToFullName();

                p.PrintLine($"/// <summary>The binding property for <see cref=\"{member.Member.Name}\"/></summary>");
                p.Print("#if UNITY_5_3_OR_NEWER").PrintEndLine();
                p.PrintLine("[global::UnityEngine.SerializeField]");
                p.Print("#endif").PrintEndLine();

                foreach (var attribute in member.ForwardedFieldAttributes)
                {
                    p.PrintLine($"[{attribute.GetSyntax().ToFullString()}]");
                }

                p.PrintLine($"[global::ZBase.Foundation.Mvvm.Label({label})]");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine(string.Format(GENERATED_BINDING_PROPERTY, ConstName(member), typeName));
                p.PrintLine($"private {readonlyKeyword}global::ZBase.Foundation.Mvvm.ViewBinding.BindingProperty {BindingPropertyName(member)} =  new global::ZBase.Foundation.Mvvm.ViewBinding.BindingProperty();");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteConverters(ref Printer p)
        {
            foreach (var member in MemberRefs)
            {
                if (member.SkipConverter)
                {
                    continue;
                }

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

                if (string.IsNullOrWhiteSpace(member.ConverterLabel))
                {
                    label = ConstName(member);
                }
                else
                {
                    label = $"\"{member.ConverterLabel}\"";
                }

                var typeName = member.NonUnionArgumentType.ToFullName();

                p.PrintLine($"/// <summary>The converter for the parameter of <see cref=\"{member.Member.Name}\"/></summary>");
                p.Print("#if UNITY_5_3_OR_NEWER").PrintEndLine();
                p.PrintLine("[global::UnityEngine.SerializeField]");
                p.Print("#endif").PrintEndLine();

                foreach (var attribute in member.ForwardedFieldAttributes)
                {
                    p.PrintLine($"[{attribute.GetSyntax().ToFullString()}]");
                }

                p.PrintLine($"[global::ZBase.Foundation.Mvvm.Label({label})]");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine(string.Format(GENERATED_CONVERTER, ConstName(member), typeName));
                p.PrintLine($"private {readonlyKeyword}global::ZBase.Foundation.Mvvm.ViewBinding.Converter {ConverterName(member)} = new global::ZBase.Foundation.Mvvm.ViewBinding.Converter();");
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
                p.PrintLine($"/// to the property chosen by {BindingPropertyName(member)}.");
                p.PrintLine($"/// </summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private readonly global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener<{className}> {ListenerName(member)};");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteFlags(ref Printer p)
        {
            p.PrintLine($"/// <summary>A flag indicates whether this binder is listening to events from <see cref=\"Context\"/>.</summary>");
            p.PrintLine(GENERATED_CODE);
            p.PrintLine($"private bool _isListening;");
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

        private void WriteStartListeningMethod(ref Printer p)
        {
            var keyword = IsBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}void StartListening()");
            p.OpenScope();
            {
                if (IsBaseBinder)
                {
                    p.PrintLine("base.StartListening();");
                    p.PrintEndLine();
                }

                p.PrintLine("if (this.Context.Target is not global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanged inpc) return;");
                p.PrintEndLine();

                p.PrintLine($"if (this._isListening) return;");
                p.PrintEndLine();

                p.PrintLine($"this._isListening = true;");
                p.PrintEndLine();

                foreach (var member in MemberRefs)
                {
                    p.PrintLine($"inpc.PropertyChanged(this.{BindingPropertyName(member)}.TargetPropertyName, this.{ListenerName(member)});");
                }
            }
            p.CloseScope();

            p.PrintEndLine();
        }

        private void WriteStopListeningMethod(ref Printer p)
        {
            var keyword = IsBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}void StopListening()");
            p.OpenScope();
            {
                if (IsBaseBinder)
                {
                    p.PrintLine("base.StopListening();");
                    p.PrintEndLine();
                }

                p.PrintLine($"if (this._isListening == false) return;");
                p.PrintEndLine();

                p.PrintLine($"this._isListening = false;");
                p.PrintEndLine();

                foreach (var member in MemberRefs)
                {
                    p.PrintLine($"this.{ListenerName(member)}.Detach();");
                }
            }
            p.CloseScope();

            p.PrintEndLine();
        }

        private void WriteSetTargetPropertyNameMethod(ref Printer p)
        {
            var keyword = IsBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}bool SetTargetPropertyName(string bindingPropertyName, string targetPropertyName)");
            p.OpenScope();
            {
                if (IsBaseBinder)
                {
                    p.PrintLine("if (base.SetTargetPropertyName(bindingPropertyName, targetPropertyName)) return true;");
                    p.PrintEndLine();
                }

                p.PrintLine("switch (bindingPropertyName)");
                p.OpenScope();
                {
                    foreach (var member in MemberRefs)
                    {
                        p.PrintLine($"case {ConstName(member)}:");
                        p.OpenScope();
                        {
                            p.PrintLine($"this.{BindingPropertyName(member)}.TargetPropertyName = targetPropertyName;");
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

        private static string ConstName(MemberRef member)
            => $"BindingProperty_{member.Member.Name}";

        private static string BindingPropertyName(MemberRef member)
            => $"_bindingFieldFor{member.Member.Name}";

        private static string ConverterName(MemberRef member)
            => $"_converterFor{member.Member.Name}";

        private static string ListenerName(MemberRef member)
            => $"_listenerFor{member.Member.Name}";

        private static string MethodName(MemberRef member)
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