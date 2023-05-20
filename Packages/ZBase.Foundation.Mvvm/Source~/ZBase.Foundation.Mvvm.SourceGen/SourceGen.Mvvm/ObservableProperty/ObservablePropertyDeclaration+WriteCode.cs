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
        private const string GENERATED_OBSERVABLE_PROPERTY = "[global::ZBase.Foundation.Mvvm.ComponentModel.SourceGen.GeneratedObservableProperty]";
        private const string GENERATED_PROPERTY_CHANGING_HANDLER = "[global::ZBase.Foundation.Mvvm.ComponentModel.SourceGen.GeneratedPropertyChangingEventHandler]";
        private const string GENERATED_PROPERTY_CHANGED_HANDLER = "[global::ZBase.Foundation.Mvvm.ComponentModel.SourceGen.GeneratedPropertyChangedEventHandler]";
        private const string UNION = "global::ZBase.Foundation.Mvvm.Unions.Union";
        private const string CACHED_UNION_CONVERTER = "global::ZBase.Foundation.Mvvm.Unions.CachedUnionConverter";

        public string WriteCodeWithoutMember()
        {
            var scopePrinter = new SyntaxNodeScopePrinter(Printer.DefaultLarge, Syntax.Parent);
            var p = scopePrinter.printer;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p = p.IncreasedIndent();

            p.PrintBeginLine();
            p.Print($"partial class {Syntax.Identifier.Text}");

            p = p.IncreasedIndent();

            if (IsBaseObservableObject)
            {
                p.Print(": global::ZBase.Foundation.Mvvm.ComponentModel.IObservableObject");
                p.Print(", global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanging");
            }
            else
            {
                p.Print(": global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanging");
            }

            p.Print(", global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanged");
            p.PrintEndLine();
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
            p.Print($"partial class {Syntax.Identifier.Text}");

            p = p.IncreasedIndent();

            if (IsBaseObservableObject)
            {
                p.Print(": global::ZBase.Foundation.Mvvm.ComponentModel.IObservableObject");
                p.Print(", global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanging");
            }
            else
            {
                p.Print(": global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanging");
            }

            p.Print(", global::ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanged");
            p.PrintEndLine();
            p = p.DecreasedIndent();

            p.OpenScope();
            {
                WriteConstantFields(ref p);
                WriteEvents(ref p);
                WriteUnionConverters(ref p);
                WriteProperties(ref p);
                WritePartialMethods(ref p);
                WritePropertyChangingMethod(ref p);
                WritePropertyChangedMethod(ref p);
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

            foreach (var type in types.Values)
            {
                var typeName = type.ToFullName();
                var propertyName = GeneratorHelpers.ToTitleCase(type.ToValidIdentifier().AsSpan());

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private readonly {CACHED_UNION_CONVERTER}<{typeName}> _unionConverter{propertyName} = new {CACHED_UNION_CONVERTER}<{typeName}>();");
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
                var converterForFieldVariable = $"unionConverter{converterForField}";
                var willNotifyPropertyChanged = NotifyPropertyChangedForMap.TryGetValue(fieldName, out var property);
                var constName = ConstName(member);

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
                        p.PrintLine($"if (global::System.Collections.Generic.EqualityComparer<{typeName}>.Default.Equals(this.{fieldName}, value)) return;");
                        p.PrintEndLine();

                        p.PrintLine($"var oldValue = this.{fieldName};");

                        if (willNotifyPropertyChanged)
                        {
                            p.PrintLine($"var oldPropertyValue = this.{property.Name};");
                        }

                        p.PrintEndLine();

                        p.PrintLine($"{OnChangingMethodName(member)}(oldValue, value);");
                        p.PrintEndLine();

                        p.PrintLine($"var {converterForFieldVariable} = this._unionConverter{converterForField};");
                        p.PrintLine($"var {argsName} = new global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventArgs(this, {constName}, {converterForFieldVariable}.ToUnion(oldValue), {converterForFieldVariable}.ToUnion(value));");
                        p.PrintLine($"this.{OnChangingEventName(member)}?.Invoke({argsName});");
                        p.PrintEndLine();

                        p.PrintLine($"this.{fieldName} = value;");
                        p.PrintEndLine();

                        p.PrintLine($"{OnChangedMethodName(member)}(oldValue, value);");
                        p.PrintLine($"this.{OnChangedEventName(member)}?.Invoke({argsName});");

                        if (willNotifyPropertyChanged)
                        {
                            var otherArgsName = OnChangedArgsName(property);
                            var converterForProperty = GeneratorHelpers.ToTitleCase(property.Type.ToValidIdentifier().AsSpan());
                            var converterForPropertyVariable = $"converter{converterForProperty}";

                            p.PrintEndLine();
                            p.PrintLine($"{OnChangedMethodName(property)}(oldPropertyValue, this.{property.Name});");
                            p.PrintEndLine();

                            p.PrintLine($"var {converterForPropertyVariable} = this._unionConverter{converterForProperty};");
                            p.PrintLine($"var {otherArgsName} = new global::ZBase.Foundation.Mvvm.ComponentModel.PropertyChangeEventArgs(this, {ConstName(property)}, {converterForPropertyVariable}.ToUnion(oldPropertyValue), {converterForPropertyVariable}.ToUnion(this.{property.Name}));");
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
                p.PrintLine("/// <param name=\"oldValue\">The previous property value that is being replaced.</param>");
                p.PrintLine("/// <param name=\"newValue\">The new property value being set.</param>");
                p.PrintLine($"/// <remarks>This method is invoked right before the value of <see cref=\"{propName}\"/> is changed.</remarks>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"partial void {OnChangingMethodName(member)}({typeName} oldValue, {typeName} newValue);");
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
                        var eventName = OnChangingEventName(member);
                        var constName = ConstName(member);

                        p.PrintLine($"case {constName}:");
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
                        var eventName = OnChangedEventName(member);
                        var constName = ConstName(member);

                        p.PrintLine($"case {constName}:");
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
                        var constName = ConstName(property);
                        var eventName = OnChangedEventName(property);

                        p.PrintLine($"case {constName}:");
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
            const string ATTRIBUTE = "[global::ZBase.Foundation.Mvvm.ComponentModel.SourceGen.NotifyPropertyChangingInfo({0}.{1}, typeof({2}))]";

            var className = Symbol.ToFullName();

            foreach (var member in MemberRefs)
            {
                var constName = ConstName(member);
                var typeName = member.Member.Type.ToFullName();

                p.PrintLine(string.Format(ATTRIBUTE, className, constName, typeName));
            }
        }

        private void WriteNotifyPropertyChangedInfoAttributes(ref Printer p)
        {
            const string ATTRIBUTE = "[global::ZBase.Foundation.Mvvm.ComponentModel.SourceGen.NotifyPropertyChangedInfo({0}.{1}, typeof({2}))]";

            var className = Symbol.ToFullName();
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

        private static string ConstName(MemberRef member)
            => $"PropertyName_{member.PropertyName}";

        private static string ConstName(IPropertySymbol member)
            => $"PropertyName_{member.Name}";

        private static string OnChangingEventName(MemberRef member)
            => $"_onChanging{member.PropertyName}";

        private static string OnChangedEventName(MemberRef member)
            => $"_onChanged{member.PropertyName}";

        private static string OnChangedEventName(ISymbol member)
            => $"_onChanged{member.Name}";

        private static string OnChangedArgsName(ISymbol member)
            => $"args{member.Name}";

        private static string OnChangedArgsName(MemberRef member)
            => $"args{member.PropertyName}";

        private static string OnChangingMethodName(MemberRef member)
            => $"On{member.PropertyName}Changing";

        private static string OnChangedMethodName(MemberRef member)
            => $"On{member.PropertyName}Changed";

        private static string OnChangedMethodName(IPropertySymbol member)
            => $"On{member.Name}Changed";
    }
}
