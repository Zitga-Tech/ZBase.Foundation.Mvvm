﻿using System;
using Microsoft.CodeAnalysis;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm.BinderSourceGen
{
    partial class BinderDeclaration
    {
        private const string AGGRESSIVE_INLINING = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]";
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.BinderGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";
        private const string OBSOLETE_METHOD = "[global::System.Obsolete(\"This method is not intended to be use directly by user code.\")]";
        private const string GENERATED_BINDING_PROPERTY = "[global::ZBase.Foundation.Mvvm.ViewBinding.SourceGen.GeneratedBindingProperty({0}, typeof({1}))]";
        private const string GENERATED_BINDING_COMMAND = "[global::ZBase.Foundation.Mvvm.ViewBinding.SourceGen.GeneratedBindingCommand(";
        private const string GENERATED_CONVERTER = "[global::ZBase.Foundation.Mvvm.ViewBinding.SourceGen.GeneratedConverter({0}, typeof({1}))]";
        private const string IADAPTER = "global::ZBase.Foundation.Mvvm.ViewBinding.IAdapter";
        private const string CACHED_UNION_CONVERTER = "global::ZBase.Foundation.Mvvm.Unions.CachedUnionConverter";

        public string WriteCode()
        {
            var scopePrinter = new SyntaxNodeScopePrinter(Printer.DefaultLarge, Syntax.Parent);
            var p = scopePrinter.printer;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p = p.IncreasedIndent();

            WriteBindingPropertyMethodInfoAttributes(ref p);
            WriteBindingCommandMethodInfoAttributes(ref p);

            p.PrintBeginLine();
            p.Print("partial class ").Print(Syntax.Identifier.Text);
            p.PrintEndLine();

            p = p.IncreasedIndent();

            if (HasBaseBinder)
            {
                p.PrintLine(": global::ZBase.Foundation.Mvvm.ViewBinding.IBinder");
            }

            p = p.DecreasedIndent();

            p.OpenScope();
            {
                WriteConstantBindingProperties(ref p);
                WriteConstantBindingCommands(ref p);
                WriteBindingProperties(ref p);
                WriteConverters(ref p);
                WriteBindingCommands(ref p);

                if (NonUnionTypes.Length > 0)
                {
                    WriteUnionConverters(ref p);
                }

                WriteListeners(ref p);
                WriteRelayCommands(ref p);
                WriteFlags(ref p);
                WriteConstructor(ref p);
                WriteStartListeningMethod(ref p);
                WriteStopListeningMethod(ref p);
                WriteSetTargetPropertyNameMethod(ref p);
                WriteSetAdapterMethod(ref p);

                if (NonUnionTypes.Length > 0)
                {
                    WriteUnionOverloads(ref p);
                }

                WritePartialBindingCommandMethods(ref p);
                WriteSetTargetCommandNameMethod(ref p);
            }
            p.CloseScope();

            p = p.DecreasedIndent();
            return p.Result;
        }

        private void WriteBindingPropertyMethodInfoAttributes(ref Printer p)
        {
            var className = Symbol.ToFullName();

            foreach (var member in BindingPropertyRefs)
            {
                p.PrintBeginLine()
                    .Print("[global::ZBase.Foundation.Mvvm.ViewBinding.SourceGen.BindingPropertyMethodInfo(")
                    .Print($"{className}.{ConstName(member)}, typeof(")
                    .Print(member.Symbol.Parameters[0].Type.ToFullName()).Print("))]")
                    .PrintEndLine();
            }
        }

        private void WriteBindingCommandMethodInfoAttributes(ref Printer p)
        {
            var className = Symbol.ToFullName();

            foreach (var member in BindingCommandRefs)
            {
                p.PrintBeginLine()
                    .Print("[global::ZBase.Foundation.Mvvm.ViewBinding.SourceGen.BindingCommandMethodInfo(")
                    .Print($"{className}.{ConstName(member)}, ");

                if (member.Parameter == null)
                {
                    p.Print("null");
                }
                else
                {
                    p.Print("typeof(").Print(member.Symbol.Parameters[0].Type.ToFullName()).Print(")");
                }

                p.Print(")]").PrintEndLine();
            }
        }

        private void WriteConstantBindingProperties(ref Printer p)
        {
            if (BindingPropertyRefs.Length < 1)
            {
                return;
            }

            var className = Syntax.Identifier.Text;

            foreach (var member in BindingPropertyRefs)
            {
                var name = member.Symbol.Name;

                p.PrintLine($"/// <summary>The name of <see cref=\"{name}\"/></summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"public const string {ConstName(member)} = nameof({className}.{name});");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteConstantBindingCommands(ref Printer p)
        {
            if (BindingCommandRefs.Length < 1)
            {
                return;
            }

            var className = Syntax.Identifier.Text;

            foreach (var member in BindingCommandRefs)
            {
                var name = member.Symbol.Name;

                p.PrintLine($"/// <summary>The name of <see cref=\"{name}\"/></summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"public const string {ConstName(member)} = nameof({className}.{name});");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteBindingProperties(ref Printer p)
        {
            if (BindingPropertyRefs.Length < 1)
            {
                return;
            }

            foreach (var member in BindingPropertyRefs)
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

                var typeName = member.Parameter.Type.ToFullName();

                p.PrintLine($"/// <summary>The binding property for <see cref=\"{member.Symbol.Name}\"/></summary>");
                p.Print("#if UNITY_5_3_OR_NEWER").PrintEndLine();
                p.PrintLine("[global::UnityEngine.SerializeField]");
                p.Print("#endif").PrintEndLine();

                foreach (var attribute in member.ForwardedFieldAttributes)
                {
                    p.PrintLine($"[{attribute.GetSyntax().ToFullString()}]");
                }

                p.PrintLine(GENERATED_CODE);
                p.PrintLine(string.Format(GENERATED_BINDING_PROPERTY, ConstName(member), typeName));
                p.PrintLine($"private {readonlyKeyword}global::ZBase.Foundation.Mvvm.ViewBinding.BindingProperty {BindingPropertyName(member)} =  new global::ZBase.Foundation.Mvvm.ViewBinding.BindingProperty();");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteConverters(ref Printer p)
        {
            if (BindingPropertyRefs.Length < 1)
            {
                return;
            }

            foreach (var member in BindingPropertyRefs)
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

                var typeName = member.Parameter.Type.ToFullName();

                p.PrintLine($"/// <summary>The converter for the parameter of <see cref=\"{member.Symbol.Name}\"/></summary>");
                p.Print("#if UNITY_5_3_OR_NEWER").PrintEndLine();
                p.PrintLine("[global::UnityEngine.SerializeField]");
                p.Print("#endif").PrintEndLine();

                foreach (var attribute in member.ForwardedFieldAttributes)
                {
                    p.PrintLine($"[{attribute.GetSyntax().ToFullString()}]");
                }

                p.PrintLine(GENERATED_CODE);
                p.PrintLine(string.Format(GENERATED_CONVERTER, ConstName(member), typeName));
                p.PrintLine($"private {readonlyKeyword}global::ZBase.Foundation.Mvvm.ViewBinding.Converter {ConverterName(member)} = new global::ZBase.Foundation.Mvvm.ViewBinding.Converter();");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteBindingCommands(ref Printer p)
        {
            if (BindingCommandRefs.Length < 1)
            {
                return;
            }

            foreach (var member in BindingCommandRefs)
            {
                if (member.SkipBindingCommand)
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

                p.PrintLine($"/// <summary>The binding command for <see cref=\"{member.Symbol.Name}\"/></summary>");
                p.Print("#if UNITY_5_3_OR_NEWER").PrintEndLine();
                p.PrintLine("[global::UnityEngine.SerializeField]");
                p.Print("#endif").PrintEndLine();

                foreach (var attribute in member.ForwardedFieldAttributes)
                {
                    p.PrintLine($"[{attribute.GetSyntax().ToFullString()}]");
                }

                p.PrintLine(GENERATED_CODE);
                p.PrintBeginLine()
                    .Print(GENERATED_BINDING_COMMAND)
                    .Print(ConstName(member));

                if (member.Parameter != null)
                {
                    p.Print($", typeof({member.Parameter.Type.ToFullName()})");
                }

                p.Print(")]").PrintEndLine();

                p.PrintLine($"private {readonlyKeyword}global::ZBase.Foundation.Mvvm.ViewBinding.BindingCommand {BindingCommandName(member)} =  new global::ZBase.Foundation.Mvvm.ViewBinding.BindingCommand();");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteUnionConverters(ref Printer p)
        {
            if (NonUnionTypes.Length < 1)
            {
                return;
            }

            foreach (var type in NonUnionTypes)
            {
                var typeName = type.ToFullName();
                var propertyName = GeneratorHelpers.ToTitleCase(type.ToValidIdentifier().AsSpan());

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private readonly {CACHED_UNION_CONVERTER}<{typeName}> _unionConverter{propertyName} = new {CACHED_UNION_CONVERTER}<{typeName}>();");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteListeners(ref Printer p)
        {
            if (BindingPropertyRefs.Length < 1)
            {
                return;
            }

            var className = Syntax.Identifier.Text;

            foreach (var member in BindingPropertyRefs)
            {
                p.PrintLine($"/// <summary>");
                p.PrintLine($"/// The listener that binds <see cref=\"{member.Symbol.Name}\"/>");
                p.PrintLine($"/// to the property chosen by <see cref=\"{BindingPropertyName(member)}\"/>.");
                p.PrintLine($"/// </summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private readonly global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener<{className}> {ListenerName(member)};");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteRelayCommands(ref Printer p)
        {
            if (BindingCommandRefs.Length < 1)
            {
                return;
            }

            foreach (var member in BindingCommandRefs)
            {
                p.PrintLine($"/// <summary>");
                p.PrintLine($"/// The relay command that binds <see cref=\"{member.Symbol.Name}\"/>");
                p.PrintLine($"/// to the command chosen by <see cref=\"{BindingCommandName(member)}\"/>.");
                p.PrintLine($"/// </summary>");
                p.PrintLine(GENERATED_CODE);

                if (member.Parameter == null)
                {
                    p.PrintLine($"private global::ZBase.Foundation.Mvvm.Input.IRelayCommand {RelayCommandName(member)};");
                }
                else
                {
                    p.PrintLine($"private global::ZBase.Foundation.Mvvm.Input.IRelayCommand<{member.Parameter.Type.ToFullName()}> {RelayCommandName(member)};");
                }

                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteFlags(ref Printer p)
        {
            if (BindingPropertyRefs.Length < 1 && BindingCommandRefs.Length < 1)
            {
                return;
            }

            p.PrintLine($"/// <summary>A flag indicates whether this binder is listening to events from <see cref=\"Context\"/>.</summary>");
            p.PrintLine(GENERATED_CODE);
            p.PrintLine($"private bool _isListening;");
            p.PrintEndLine();
        }

        private void WriteConstructor(ref Printer p)
        {
            if (BindingPropertyRefs.Length < 1)
            {
                return;
            }

            var className = Syntax.Identifier.Text;

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintBeginLine().Print($"public {className}()");

            if (HasBaseBinder)
            {
                p.Print(" : base()");
            }

            p.PrintEndLine();

            p.OpenScope();
            {
                foreach (var member in BindingPropertyRefs)
                {
                    var methodName = MethodName(member);

                    p.PrintLine($"this.{ListenerName(member)} = new global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener<{className}>(this)");
                    p.OpenScope();
                    p.PrintLine($"OnEventAction = (instance, args) => instance.{methodName}(this.{ConverterName(member)}.Convert(args.NewValue))");
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
            var keyword = HasBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            if (BindingPropertyRefs.Length < 1 && BindingCommandRefs.Length < 1)
            {
                if (HasBaseBinder == false)
                {
                    p.PrintLine("/// <inheritdoc/>");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}void StartListening() {{ }}");
                    p.PrintEndLine();
                }

                return;
            }

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}void StartListening()");
            p.OpenScope();
            {
                if (HasBaseBinder)
                {
                    p.PrintLine("base.StartListening();");
                    p.PrintEndLine();
                }

                p.PrintLine($"if (this._isListening) return;");
                p.PrintEndLine();

                p.PrintLine($"this._isListening = true;");
                p.PrintEndLine();

                if (BindingPropertyRefs.Length > 0)
                {
                    p.PrintLine("if (this.Context.Target is global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanged inpc)");
                    p.OpenScope();
                    {
                        foreach (var member in BindingPropertyRefs)
                        {
                            p.PrintLine($"inpc.PropertyChanged(this.{BindingPropertyName(member)}.TargetPropertyName, this.{ListenerName(member)});");
                        }
                    }
                    p.CloseScope();
                    p.PrintEndLine();
                }

                if (BindingCommandRefs.Length > 0)
                {
                    p.PrintLine("if (this.Context.Target is global::ZBase.Foundation.Mvvm.Input.ICommandListener cl)");
                    p.OpenScope();
                    {
                        foreach (var member in BindingCommandRefs)
                        {
                            p.PrintLine($"cl.TryGetCommand(this.{BindingCommandName(member)}.TargetCommandName, out this.{RelayCommandName(member)});");
                        }
                    }
                    p.CloseScope();
                }
            }
            p.CloseScope();

            p.PrintEndLine();
        }

        private void WriteStopListeningMethod(ref Printer p)
        {
            var keyword = HasBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            if (BindingPropertyRefs.Length < 1 && BindingCommandRefs.Length < 1)
            {
                if (HasBaseBinder == false)
                {
                    p.PrintLine("/// <inheritdoc/>");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}void StopListening() {{ }}");
                    p.PrintEndLine();
                }

                return;
            }

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}void StopListening()");
            p.OpenScope();
            {
                if (HasBaseBinder)
                {
                    p.PrintLine("base.StopListening();");
                    p.PrintEndLine();
                }

                p.PrintLine($"if (this._isListening == false) return;");
                p.PrintEndLine();

                p.PrintLine($"this._isListening = false;");
                p.PrintEndLine();

                if (BindingPropertyRefs.Length > 0)
                {
                    foreach (var member in BindingPropertyRefs)
                    {
                        p.PrintLine($"this.{ListenerName(member)}.Detach();");
                    }

                    p.PrintEndLine();
                }

                if (BindingCommandRefs.Length > 0)
                {
                    foreach (var member in BindingCommandRefs)
                    {
                        p.PrintLine($"this.{RelayCommandName(member)} = null;");
                    }
                }
            }
            p.CloseScope();

            p.PrintEndLine();
        }

        private void WriteSetTargetPropertyNameMethod(ref Printer p)
        {
            var keyword = HasBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            if (BindingPropertyRefs.Length < 1)
            {
                if (HasBaseBinder == false)
                {
                    p.PrintLine("/// <inheritdoc/>");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}bool SetTargetPropertyName(string bindingPropertyName, string targetPropertyName)");
                    p.OpenScope();
                    {
                        p.PrintLine("return false;");
                    }
                    p.CloseScope();
                    p.PrintEndLine();
                }

                return;
            }

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}bool SetTargetPropertyName(string bindingPropertyName, string targetPropertyName)");
            p.OpenScope();
            {
                if (HasBaseBinder)
                {
                    p.PrintLine("if (base.SetTargetPropertyName(bindingPropertyName, targetPropertyName)) return true;");
                    p.PrintEndLine();
                }

                p.PrintLine("switch (bindingPropertyName)");
                p.OpenScope();
                {
                    foreach (var member in BindingPropertyRefs)
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

        private void WriteSetAdapterMethod(ref Printer p)
        {
            var keyword = HasBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            if (BindingPropertyRefs.Length < 1)
            {
                if (HasBaseBinder == false)
                {
                    p.PrintLine("/// <inheritdoc/>");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}bool SetAdapter(string bindingPropertyName, {IADAPTER} adapter)");
                    p.OpenScope();
                    {
                        p.PrintLine("return false;");
                    }
                    p.CloseScope();
                    p.PrintEndLine();
                }

                return;
            }

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}bool SetAdapter(string bindingPropertyName, {IADAPTER} adapter)");
            p.OpenScope();
            {
                if (HasBaseBinder)
                {
                    p.PrintLine("if (base.SetAdapter(bindingPropertyName, adapter)) return true;");
                    p.PrintEndLine();
                }

                p.PrintLine("switch (bindingPropertyName)");
                p.OpenScope();
                {
                    foreach (var member in BindingPropertyRefs)
                    {
                        p.PrintLine($"case {ConstName(member)}:");
                        p.OpenScope();
                        {
                            p.PrintLine($"this.{ConverterName(member)}.Adapter = adapter;");
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
            if (BindingPropertyRefs.Length < 1)
            {
                return;
            }

            foreach (var member in BindingPropertyRefs)
            {
                if (member.IsParameterTypeNotUnion == false)
                {
                    continue;
                }

                var originalMethodName = member.Symbol.Name;
                var param = member.Parameter;
                var paramTypeName = param.Type.ToFullName();
                var methodName = MethodName(member);
                var converter = GeneratorHelpers.ToTitleCase(param.Type.ToValidIdentifier().AsSpan());

                p.PrintLine($"/// <summary>");
                p.PrintLine($"/// This overload will try to get the value of type <see cref=\"{paramTypeName}\"/>");
                p.PrintLine($"/// from <see cref=\"global::ZBase.Foundation.Mvvm.Unions.Union\"/>");
                p.PrintLine($"/// to pass into <see cref=\"{originalMethodName}\"/>.");
                p.PrintLine($"/// </summary>");
                p.PrintLine($"/// <remarks>This method is not intended to be use directly by user code.</remarks>");
                p.PrintLine(OBSOLETE_METHOD).PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                p.PrintLine($"private void {methodName}(in global::ZBase.Foundation.Mvvm.Unions.Union union)");
                p.OpenScope();
                {
                    p.PrintLine($"if (this._unionConverter{converter}.TryGetValue(union, out {paramTypeName} value))");
                    p.OpenScope();
                    {
                        p.PrintBeginLine().Print($"{originalMethodName}(");

                        if (param.RefKind == RefKind.Ref)
                        {
                            p.Print("ref ");
                        }

                        p.Print("value);").PrintEndLine();
                    }
                    p.CloseScope();
                }
                p.CloseScope();
            }

            p.PrintEndLine();
        }

        private void WritePartialBindingCommandMethods(ref Printer p)
        {
            if (BindingCommandRefs.Length < 1)
            {
                return;
            }

            foreach (var member in BindingCommandRefs)
            {
                p.PrintLine(AGGRESSIVE_INLINING).PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);

                if (member.Parameter == null)
                {
                    p.PrintLine($"partial void {member.Symbol.Name}()");
                    p.OpenScope();
                    {
                        p.PrintLine($"this.{RelayCommandName(member)}?.Execute();");
                    }
                    p.CloseScope();
                }
                else
                {
                    p.PrintBeginLine().Print($"partial void {member.Symbol.Name}(");

                    var param = member.Parameter;

                    if (param.RefKind == RefKind.Ref)
                    {
                        p.Print("ref ");
                    }

                    p.Print($"{param.Type.ToFullName()} {param.Name})").PrintEndLine();
                    p.OpenScope();
                    {
                        p.PrintLine($"this.{RelayCommandName(member)}?.Execute({param.Name});");
                    }
                    p.CloseScope();
                }
                
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteSetTargetCommandNameMethod(ref Printer p)
        {
            var keyword = HasBaseBinder ? "override " : Symbol.IsSealed ? "" : "virtual ";

            if (BindingCommandRefs.Length < 1)
            {
                if (HasBaseBinder == false)
                {
                    p.PrintLine("/// <inheritdoc/>");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}bool SetTargetCommandName(string bindingCommandName, string targetCommandName)");
                    p.OpenScope();
                    {
                        p.PrintLine("return false;");
                    }
                    p.CloseScope();
                    p.PrintEndLine();
                }

                return;
            }

            p.PrintLine("/// <inheritdoc/>");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}bool SetTargetCommandName(string bindingCommandName, string targetCommandName)");
            p.OpenScope();
            {
                if (HasBaseBinder)
                {
                    p.PrintLine("if (base.SetTargetCommandName(bindingCommandName, targetCommandName)) return true;");
                    p.PrintEndLine();
                }

                p.PrintLine("switch (bindingCommandName)");
                p.OpenScope();
                {
                    foreach (var member in BindingCommandRefs)
                    {
                        p.PrintLine($"case {ConstName(member)}:");
                        p.OpenScope();
                        {
                            p.PrintLine($"this.{BindingCommandName(member)}.TargetCommandName = targetCommandName;");
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

        private static string ConstName(BindingPropertyRef member)
            => $"BindingProperty_{member.Symbol.Name}";

        private static string ConstName(BindingCommandRef member)
            => $"BindingCommand_{member.Symbol.Name}";

        private static string BindingPropertyName(BindingPropertyRef member)
            => $"_bindingFieldFor{member.Symbol.Name}";

        private static string ConverterName(BindingPropertyRef member)
            => $"_converterFor{member.Symbol.Name}";

        private static string ListenerName(BindingPropertyRef member)
            => $"_listenerFor{member.Symbol.Name}";

        private static string MethodName(BindingPropertyRef member)
        {
            var name = member.Symbol.Name;

            if (member.IsParameterTypeNotUnion)
            {
                return $"{name}__Union";
            }

            return name;
        }

        private static string BindingCommandName(BindingCommandRef member)
            => $"_bindingCommandFor{member.Symbol.Name}";

        private static string RelayCommandName(BindingCommandRef member)
            => $"_relayCommandFor{member.Symbol.Name}";
    }
}