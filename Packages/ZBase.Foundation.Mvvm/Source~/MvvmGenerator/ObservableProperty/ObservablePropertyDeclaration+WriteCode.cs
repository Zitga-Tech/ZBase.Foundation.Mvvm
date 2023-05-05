using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm.ObservablePropertySourceGen
{
    partial class ObservablePropertyDeclaration
    {
        private const string AGGRESSIVE_INLINING = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]";
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.ObservablePropertyGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";
        private const string GENERATED_OBSERVABLE_PROPERTY = "[global::ZBase.Foundation.Mvvm.ComponentModel.GeneratedObservableProperty]";
        private const string GENERATED_PROPERTY_CHANGING_HANDLER = "[global::ZBase.Foundation.Mvvm.ComponentModel.GeneratedPropertyChangingEventHandler]";
        private const string GENERATED_PROPERTY_CHANGED_HANDLER = "[global::ZBase.Foundation.Mvvm.ComponentModel.GeneratedPropertyChangedEventHandler]";

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

            if (IsBaseObservableObject)
            {
                p.PrintLine(": global::ZBase.Foundation.Mvvm.ComponentModel.IObservableObject");
                p.PrintLine(", global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanging");
            }
            else
            {
                p.PrintLine(": global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanging");
            }

            p.PrintLine(", global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanged");
            p = p.DecreasedIndent();

            p.OpenScope();
            {
                if (IsBaseObservableObject == false)
                {
                    var keyword = Symbol.IsSealed ? "" : "virtual ";

                    p.PrintLine($"/// <inheritdoc cref=\"global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanging.PropertyChanging{{TInstance}}(string, global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener{{TInstance}})\" />");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}bool PropertyChanging<TInstance>(string propertyName, global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener<TInstance> listener) where TInstance : class");
                    p.OpenScope();
                    {
                        if (IsBaseObservableObject)
                        {
                            p.PrintLine("return base.PropertyChanging<TInstance>(propertyName, listener);");
                        }
                        else
                        {
                            p.PrintLine("return false;");
                        }
                    }
                    p.CloseScope();

                    p.PrintEndLine();

                    p.PrintLine($"/// <inheritdoc cref=\"global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanged.PropertyChanged{{TInstance}}(string, global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener{{TInstance}})\" />");
                    p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public {keyword}bool PropertyChanged<TInstance>(string propertyName, global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener<TInstance> listener) where TInstance : class");
                    p.OpenScope();
                    {
                        if (IsBaseObservableObject)
                        {
                            p.PrintLine("return base.PropertyChanged<TInstance>(propertyName, listener);");
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
            p = p.IncreasedIndent();

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            WriteNotifyPropertyChangingInfoAttributes(ref p);
            WriteNotifyPropertyChangedInfoAttributes(ref p);

            p.PrintBeginLine();
            p.Print("partial class ").Print(Syntax.Identifier.Text);
            p.PrintEndLine();

            p = p.IncreasedIndent();

            if (IsBaseObservableObject)
            {
                p.PrintLine(": global::ZBase.Foundation.Mvvm.ComponentModel.IObservableObject");
                p.PrintLine(", global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanging");
            }
            else
            {
                p.PrintLine(": global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanging");
            }

            p.PrintLine(", global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanged");
            p = p.DecreasedIndent();

            p.OpenScope();
            {
                p.PrintLine(GENERATED_CODE);
                p.PrintLine("private readonly UnionConverters _unionConverters = new UnionConverters();");
                p.PrintEndLine();

                WriteConstantFields(ref p);
                WriteEvents(ref p);
                WriteProperties(ref p);
                WritePartialMethods(ref p);
                WritePropertyChangingMethod(ref p);
                WritePropertyChangedMethod(ref p);
                WriteUnionConverters(ref p);
            }
            p.CloseScope();

            p = p.DecreasedIndent();
            return p.Result;
        }

        private void WriteConstantFields(ref Printer p)
        {
            var className = Syntax.Identifier.Text;
            var additionalProperties = new Dictionary<string, IPropertySymbol>();

            foreach (var member in MemberRefs)
            {
                var fieldName = member.Member.Name;
                var name = member.PropertyName;

                p.PrintLine($"/// <summary>The name of <see cref=\"{name}\"/></summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"public const string {ConstName(member)} = nameof({className}.{name});");
                p.PrintEndLine();

                if (NotifyPropertyChangedForMap.TryGetValue(fieldName, out var property))
                {
                    if (additionalProperties.ContainsKey(property.Name) == false)
                    {
                        additionalProperties[property.Name] = property;
                    }
                }
            }

            foreach (var property in additionalProperties.Values)
            {
                var name = property.Name;

                p.PrintLine($"/// <summary>The name of <see cref=\"{name}\"/></summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"public const string {ConstName(property)} = nameof({className}.{name});");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteEvents(ref Printer p)
        {
            foreach (var member in MemberRefs)
            {
                p.PrintLine(GENERATED_CODE).PrintLine(GENERATED_PROPERTY_CHANGING_HANDLER);
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangingEventHandler {OnChangingEventName(member)};");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE).PrintLine(GENERATED_PROPERTY_CHANGED_HANDLER);
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangedEventHandler {OnChangedEventName(member)};");
                p.PrintEndLine();
            }

            var properties = new Dictionary<string, IPropertySymbol>();

            foreach (var property in NotifyPropertyChangedForMap.Values)
            {
                if (properties.ContainsKey(property.Name) == false)
                {
                    properties[property.Name] = property;
                }
            }

            foreach (var property in properties.Values)
            {
                p.PrintLine(GENERATED_CODE).PrintLine(GENERATED_PROPERTY_CHANGED_HANDLER);
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangedEventHandler {OnChangedEventName(property)};");
                p.PrintEndLine();
            }
        }

        private void WriteProperties(ref Printer p)
        {
            foreach (var member in MemberRefs)
            {
                var fieldName = member.Member.Name;
                var propertyName = member.PropertyName;
                var typeName = member.Member.Type.ToFullName();
                var argsName = OnChangedArgsName(member);
                var converterForField = GeneratorHelpers.ToTitleCase(member.Member.Type.ToValidIdentifier().AsSpan());
                var willNotifyPropertyChanged = NotifyPropertyChangedForMap.TryGetValue(fieldName, out var property);

                p.PrintLine($"/// <inheritdoc cref=\"{fieldName}\"/>");

                foreach (var attribute in member.ForwardedPropertyAttributes)
                {
                    p.PrintLine($"[{attribute.GetSyntax().ToFullString()}]");
                }

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(GENERATED_OBSERVABLE_PROPERTY);
                p.PrintLine($"public {typeName} {propertyName}");
                p.OpenScope();
                {
                    p.PrintLine(AGGRESSIVE_INLINING);
                    p.PrintLine($"get => this.{fieldName};");
                    p.PrintLine("set");
                    p.OpenScope();
                    {
                        p.PrintLine($"if (global::System.Collections.Generic.EqualityComparer<{typeName}>.Default.Equals(this.{fieldName}, value) == false)");
                        p.OpenScope();
                        {
                            p.PrintLine($"var oldValue = this.{fieldName};");

                            if (willNotifyPropertyChanged)
                            {
                                p.PrintLine($"var oldPropertyValue = this.{property.Name};");
                            }

                            p.PrintEndLine();

                            p.PrintLine($"{OnChangingMethodName(member)}(value);");
                            p.PrintLine($"{OnChangingMethodName(member)}(oldValue, value);");
                            p.PrintEndLine();

                            p.PrintLine($"var {argsName} = new global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventArgs(this, nameof(this.{propertyName}), this._unionConverters.{converterForField}.ToUnion(value));");
                            p.PrintLine($"this.{OnChangingEventName(member)}?.Invoke({argsName});");
                            p.PrintEndLine();

                            p.PrintLine($"this.{fieldName} = value;");
                            p.PrintEndLine();

                            p.PrintLine($"{OnChangedMethodName(member)}(value);");
                            p.PrintLine($"{OnChangedMethodName(member)}(oldValue, value);");
                            p.PrintLine($"this.{OnChangedEventName(member)}?.Invoke({argsName});");

                            if (willNotifyPropertyChanged)
                            {
                                var otherArgsName = OnChangedArgsName(property);
                                var converterForProperty = GeneratorHelpers.ToTitleCase(property.Type.ToValidIdentifier().AsSpan());

                                p.PrintEndLine();
                                p.PrintLine($"{OnChangedMethodName(property)}(this.{property.Name});");
                                p.PrintLine($"{OnChangedMethodName(property)}(oldPropertyValue, this.{property.Name});");
                                p.PrintEndLine();

                                p.PrintLine($"var {otherArgsName} = new global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventArgs(this, nameof(this.{property.Name}), this._unionConverters.{converterForProperty}.ToUnion(this.{property.Name}));");
                                p.PrintLine($"this.{OnChangedEventName(property)}?.Invoke({otherArgsName});");
                            }

                            p.PrintEndLine();

                            foreach (var commandName in member.CommandNames)
                            {
                                if (NotifyCanExecuteChangedForSet.Contains(commandName))
                                {
                                    p.PrintLine($"{commandName}.NotifyCanExecuteChanged();");
                                }
                            }
                        }
                        p.CloseScope();

                    }
                    p.CloseScope();
                }
                p.CloseScope();
                p.PrintEndLine();
            }
        }

        private void WritePartialMethods(ref Printer p)
        {
            foreach (var member in MemberRefs)
            {
                var typeName = member.Member.Type.ToFullName();
                var propName = member.PropertyName;

                p.PrintLine($"/// <summary>Executes the logic for when <see cref=\"{propName}\"/> is changing.</summary>");
                p.PrintLine("/// <param name=\"value\">The new property value being set.</param>");
                p.PrintLine($"/// <remarks>This method is invoked right before the value of <see cref=\"{propName}\"/> is changed.</remarks>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"partial void {OnChangingMethodName(member)}({typeName} value);");
                p.PrintEndLine();

                p.PrintLine($"/// <summary>Executes the logic for when <see cref=\"{propName}\"/> is changing.</summary>");
                p.PrintLine("/// <param name=\"oldValue\">The previous property value that is being replaced.</param>");
                p.PrintLine("/// <param name=\"newValue\">The new property value being set.</param>");
                p.PrintLine($"/// <remarks>This method is invoked right before the value of <see cref=\"{propName}\"/> is changed.</remarks>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"partial void {OnChangingMethodName(member)}({typeName} oldValue, {typeName} newValue);");
                p.PrintEndLine();

                p.PrintLine($"/// <summary>Executes the logic for when <see cref=\"{propName}\"/> just changed.</summary>");
                p.PrintLine("/// <param name=\"value\">The new property value that was set.</param>");
                p.PrintLine($"/// <remarks>This method is invoked right after the value of <see cref=\"{propName}\"/> is changed.</remarks>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"partial void {OnChangedMethodName(member)}({typeName} value);");
                p.PrintEndLine();

                p.PrintLine($"/// <summary>Executes the logic for when <see cref=\"{propName}\"/> just changed.</summary>");
                p.PrintLine("/// <param name=\"oldValue\">The previous property value that was replaced.</param>");
                p.PrintLine("/// <param name=\"newValue\">The new property value that was set.</param>");
                p.PrintLine($"/// <remarks>This method is invoked right after the value of <see cref=\"{propName}\"/> is changed.</remarks>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"partial void {OnChangedMethodName(member)}({typeName} oldValue, {typeName} newValue);");
                p.PrintEndLine();
            }

            var properties = new Dictionary<string, IPropertySymbol>();

            foreach (var property in NotifyPropertyChangedForMap.Values)
            {
                if (properties.ContainsKey(property.Name) == false)
                {
                    properties[property.Name] = property;
                }
            }

            foreach (var property in properties.Values)
            {
                var propName = property.Name;
                var typeName = property.Type.ToFullName();

                p.PrintLine($"/// <summary>Executes the logic for when <see cref=\"{propName}\"/> just changed.</summary>");
                p.PrintLine("/// <param name=\"value\">The new property value that was set.</param>");
                p.PrintLine($"/// <remarks>This method is invoked right after the value of <see cref=\"{propName}\"/> is changed.</remarks>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"partial void {OnChangedMethodName(property)}({typeName} value);");
                p.PrintEndLine();

                p.PrintLine($"/// <summary>Executes the logic for when <see cref=\"{propName}\"/> just changed.</summary>");
                p.PrintLine("/// <param name=\"value\">The new property value that was set.</param>");
                p.PrintLine($"/// <remarks>This method is invoked right after the value of <see cref=\"{propName}\"/> is changed.</remarks>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"partial void {OnChangedMethodName(property)}({typeName} oldValue, {typeName} newValue);");
                p.PrintEndLine();
            }
        }

        private void WritePropertyChangingMethod(ref Printer p)
        {
            var keyword = IsBaseObservableObject ? "override " : Symbol.IsSealed ? "" : "virtual ";

            p.PrintLine($"/// <inheritdoc cref=\"global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanging.PropertyChanging{{TInstance}}(string, global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener{{TInstance}})\" />");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}bool PropertyChanging<TInstance>(string propertyName, global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener<TInstance> listener) where TInstance : class");
            p.OpenScope();
            {
                if (IsBaseObservableObject)
                {
                    p.PrintLine("if (base.PropertyChanging<TInstance>(propertyName, listener)) return true;");
                }
                else
                {
                    p.PrintLine("if (propertyName == null) throw new global::System.ArgumentNullException(nameof(propertyName));");
                    p.PrintLine("if (listener == null) throw new global::System.ArgumentNullException(nameof(listener));");
                }

                p.PrintEndLine();

                p.PrintLine("switch (propertyName)");
                p.OpenScope();
                {
                    foreach (var member in MemberRefs)
                    {
                        var propertyName = member.PropertyName;
                        var eventName = OnChangingEventName(member);

                        p.PrintLine($"case nameof(this.{propertyName}):");
                        p.OpenScope();
                        {
                            p.PrintLine($"this.{eventName} += listener.OnEvent;");
                            p.PrintLine($"listener.OnDetachAction = (listener) => this.{eventName} -= listener.OnEvent;");
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

        private void WritePropertyChangedMethod(ref Printer p)
        {
            var keyword = IsBaseObservableObject ? "override " : Symbol.IsSealed ? "" : "virtual ";

            p.PrintLine($"/// <inheritdoc cref=\"global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanged.PropertyChanged{{TInstance}}(string, global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener{{TInstance}})\" />");
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine($"public {keyword}bool PropertyChanged<TInstance>(string propertyName, global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventListener<TInstance> listener) where TInstance : class");
            p.OpenScope();
            {
                if (IsBaseObservableObject)
                {
                    p.PrintLine("if (base.PropertyChanged<TInstance>(propertyName, listener)) return true;");
                }
                else
                {
                    p.PrintLine("if (propertyName == null) throw new global::System.ArgumentNullException(nameof(propertyName));");
                    p.PrintLine("if (listener == null) throw new global::System.ArgumentNullException(nameof(listener));");
                }

                p.PrintEndLine();

                p.PrintLine("switch (propertyName)");
                p.OpenScope();
                {
                    foreach (var member in MemberRefs)
                    {
                        var propertyName = member.PropertyName;
                        var eventName = OnChangedEventName(member);

                        p.PrintLine($"case nameof(this.{propertyName}):");
                        p.OpenScope();
                        {
                            p.PrintLine($"this.{eventName} += listener.OnEvent;");
                            p.PrintLine($"listener.OnDetachAction = (listener) => this.{eventName} -= listener.OnEvent;");
                            p.PrintLine("return true;");
                        }
                        p.CloseScope();
                        p.PrintEndLine();
                    }

                    var properties = new Dictionary<string, IPropertySymbol>();

                    foreach (var property in NotifyPropertyChangedForMap.Values)
                    {
                        if (properties.ContainsKey(property.Name) == false)
                        {
                            properties[property.Name] = property;
                        }
                    }

                    foreach (var property in properties.Values)
                    {
                        var propertyName = property.Name;
                        var eventName = OnChangedEventName(property);

                        p.PrintLine($"case nameof(this.{propertyName}):");
                        p.OpenScope();
                        {
                            p.PrintLine($"this.{eventName} += listener.OnEvent;");
                            p.PrintLine($"listener.OnDetachAction = (listener) => this.{eventName} -= listener.OnEvent;");
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

        private void WriteNotifyPropertyChangingInfoAttributes(ref Printer p)
        {
            const string ATTRIBUTE = "[global::ZBase.Foundation.Mvvm.ComponentModel.NotifyPropertyChangingInfo({0}.{1}, typeof({2}))]";

            var className = Syntax.Identifier.Text;

            foreach (var member in MemberRefs)
            {
                var constName = ConstName(member);
                var typeName = member.Member.Type.ToFullName();

                p.PrintLine(string.Format(ATTRIBUTE, className, constName, typeName));
            }
        }

        private void WriteNotifyPropertyChangedInfoAttributes(ref Printer p)
        {
            const string ATTRIBUTE = "[global::ZBase.Foundation.Mvvm.ComponentModel.NotifyPropertyChangedInfo({0}.{1}, typeof({2}))]";

            var className = Syntax.Identifier.Text;
            var additionalProperties = new Dictionary<string, IPropertySymbol>();

            foreach (var member in MemberRefs)
            {
                var fieldName = member.Member.Name;
                var constName = ConstName(member);
                var typeName = member.Member.Type.ToFullName();

                p.PrintLine(string.Format(ATTRIBUTE, className, constName, typeName));

                if (NotifyPropertyChangedForMap.TryGetValue(fieldName, out var property))
                {
                    if (additionalProperties.ContainsKey(property.Name) == false)
                    {
                        additionalProperties[property.Name] = property;
                    }
                }
            }

            foreach (var property in additionalProperties.Values)
            {
                p.PrintLine(string.Format(ATTRIBUTE, className, ConstName(property), property.Type.ToFullName()));
            }
        }

        private void WriteUnionConverters(ref Printer p)
        {
            var types = new Dictionary<string, ITypeSymbol>();

            foreach (var member in MemberRefs)
            {
                var fieldName = member.Member.Name;
                var name = member.Member.Type.ToFullName();

                if (types.ContainsKey(name) == false)
                {
                    types.Add(name, member.Member.Type);
                }

                if (NotifyPropertyChangedForMap.TryGetValue(fieldName, out var property))
                {
                    name = property.Type.ToFullName();

                    if (types.ContainsKey(name) == false)
                    {
                        types.Add(name, property.Type);
                    }
                }
            }

            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine("private class UnionConverters");
            p.OpenScope();
            {
                foreach (var type in types.Values)
                {
                    var typeName = type.ToFullName();
                    var fieldName = type.ToValidIdentifier();

                    p.PrintLine(GENERATED_CODE);
                    p.PrintLine($"private global::ZBase.Foundation.Mvvm.Unions.IUnionConverter<{typeName}> _{fieldName};");
                    p.PrintEndLine();
                }

                foreach (var type in types.Values)
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
            => $"PropertyName_{member.PropertyName}";

        private string ConstName(IPropertySymbol member)
            => $"PropertyName_{member.Name}";

        private string OnChangingEventName(MemberRef member)
            => $"_onChanging{member.PropertyName}";

        private string OnChangedEventName(MemberRef member)
            => $"_onChanged{member.PropertyName}";

        private string OnChangedEventName(ISymbol member)
            => $"_onChanged{member.Name}";

        private string OnChangedArgsName(ISymbol member)
            => $"args{member.Name}";

        private string OnChangedArgsName(MemberRef member)
            => $"args{member.PropertyName}";

        private string OnChangingMethodName(MemberRef member)
            => $"On{member.PropertyName}Changing";

        private string OnChangedMethodName(MemberRef member)
            => $"On{member.PropertyName}Changed";

        private string OnChangedMethodName(IPropertySymbol member)
            => $"On{member.Name}Changed";
    }
}
