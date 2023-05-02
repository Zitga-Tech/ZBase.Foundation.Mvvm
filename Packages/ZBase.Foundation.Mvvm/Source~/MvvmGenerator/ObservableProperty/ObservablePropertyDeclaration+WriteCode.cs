using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    partial class ObservablePropertyDeclaration
    {
        private const string AGGRESSIVE_INLINING = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]";
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.ObservablePropertyGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";

        public string WriteCode()
        {
            var scopePrinter = new SyntaxNodeScopePrinter(Printer.DefaultLarge, Syntax.Parent);
            var p = scopePrinter.printer;
            p = p.IncreasedIndent();

            p.PrintLine("#pragma warning disable");

            WriteNotifyPropertyChangingAttributes(ref p);
            WriteNotifyPropertyChangedAttributes(ref p);

            p.PrintBeginLine();
            p.Print("partial class ").Print(Syntax.Identifier.Text);
            p.PrintEndLine();

            p = p.IncreasedIndent();
            p.PrintLine(": global::ZBase.Foundation.Mvvm.INotifyPropertyChanging");
            p.PrintLine(", global::ZBase.Foundation.Mvvm.INotifyPropertyChanged");
            p = p.DecreasedIndent();

            p.OpenScope();
            {
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

        private void WriteEvents(ref Printer p)
        {
            foreach (var member in Members)
            {
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler {OnChangingEventName(member)};");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler {OnChangedEventName(member)};");
                p.PrintEndLine();
            }

            foreach (var member in NotifyPropertyChangedForMap.Values)
            {
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler {OnChangedEventName(member)};");
                p.PrintEndLine();
            }
        }

        private void WriteProperties(ref Printer p)
        {
            foreach (var member in Members)
            {
                var fieldName = member.Member.Name;
                var propertyName = member.PropertyName;
                var typeName = member.Member.Type.ToFullName();
                var argsName = OnChangedArgsName(member);
                var converterForField = GeneratorHelpers.ToTitleCase(member.Member.Type.ToValidIdentifier().AsSpan());

                p.PrintLine(GENERATED_CODE);
                p.PrintLine(EXCLUDE_COVERAGE);
                p.PrintLine($"public {typeName} {propertyName}");
                p.OpenScope();
                {
                    p.PrintLine(AGGRESSIVE_INLINING);
                    p.PrintLine($"get => {fieldName};");
                    p.PrintLine("set");
                    p.OpenScope();
                    {
                        p.PrintLine($"if (global::System.Collections.Generic.EqualityComparer<{typeName}>.Default.Equals({fieldName}, value) == false)");
                        p.OpenScope();
                        {
                            p.PrintLine($"{OnChangingMethodName(member)}(value);");
                            p.PrintLine($"var {argsName} = new global::ZBase.Foundation.Mvvm.PropertyChangeEventArgs(this, nameof(this.{propertyName}), UnionConverters.{converterForField}.ToUnion(value));");
                            p.PrintLine($"this.{OnChangingEventName(member)}?.Invoke({argsName});");
                            p.PrintLine($"this.{fieldName} = value;");
                            p.PrintLine($"{OnChangedMethodName(member)}(value);");
                            p.PrintLine($"this.{OnChangedEventName(member)}?.Invoke({argsName});");

                            if (NotifyPropertyChangedForMap.TryGetValue(fieldName, out var property))
                            {
                                var otherArgsName = OnChangedArgsName(property);
                                var converterForProperty = GeneratorHelpers.ToTitleCase(property.Type.ToValidIdentifier().AsSpan());

                                p.PrintEndLine();
                                p.PrintLine($"{OnChangedMethodName(property)}(this.{propertyName});");
                                p.PrintLine($"var {otherArgsName} = new global::ZBase.Foundation.Mvvm.PropertyChangeEventArgs(this, nameof(this.{propertyName}), UnionConverters.{converterForProperty}.ToUnion(this.{propertyName}));");
                                p.PrintLine($"this.{OnChangedEventName(property)}?.Invoke({otherArgsName});");
                            }

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
            foreach (var member in Members)
            {
                var typeName = member.Member.Type.ToFullName();

                p.PrintLine($"/// <summary>Executes the logic for when <see cref=\"{member.PropertyName}\"/> is changing.</summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"partial void {OnChangingMethodName(member)}({typeName} value);");
                p.PrintEndLine();

                p.PrintLine($"/// <summary>Executes the logic for when <see cref=\"{member.PropertyName}\"/> just changed.</summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"partial void {OnChangedMethodName(member)}({typeName} value);");
                p.PrintEndLine();
            }

            foreach (var member in NotifyPropertyChangedForMap.Values)
            {
                p.PrintLine($"/// <summary>Executes the logic for when <see cref=\"{member.Name}\"/> just changed.</summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"partial void {OnChangedMethodName(member)}({member.Type.ToFullName()} value);");
                p.PrintEndLine();
            }
        }

        private void WritePropertyChangingMethod(ref Printer p)
        {
            p.PrintLine($"/// <inheritdoc cref=\"global::ZBase.Foundation.Mvvm.INotifyPropertyChanging.PropertyChanging{{TInstance}}(string, global::ZBase.Foundation.Mvvm.PropertyChangeEventListener{{TInstance}})\" />");
            p.PrintLine(GENERATED_CODE);
            p.PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine("public void PropertyChanging<TInstance>(string propertyName, global::ZBase.Foundation.Mvvm.PropertyChangeEventListener<TInstance> listener) where TInstance : class");
            p.OpenScope();
            {
                p.PrintLine("if (propertyName == null) throw new global::System.ArgumentNullException(nameof(propertyName));");
                p.PrintLine("if (listener == null) throw new global::System.ArgumentNullException(nameof(listener));");
                p.PrintEndLine();

                p.PrintLine("switch (propertyName)");
                p.OpenScope();
                {
                    foreach (var member in Members)
                    {
                        var propertyName = member.PropertyName;
                        var eventName = OnChangingEventName(member);

                        p.PrintLine($"case nameof(this.{propertyName}):");
                        p.OpenScope();
                        {
                            p.PrintLine($"this.{eventName} += listener.OnEvent;");
                            p.PrintLine($"listener.OnDetachAction = (listener) => this.{eventName} -= listener.OnEvent;");
                            p.PrintLine("break;");
                        }
                        p.CloseScope();
                        p.PrintEndLine();
                    }
                }
                p.CloseScope();
            }
            p.CloseScope();
            p.PrintEndLine();
        }

        private void WritePropertyChangedMethod(ref Printer p)
        {
            p.PrintLine($"/// <inheritdoc cref=\"global::ZBase.Foundation.Mvvm.INotifyPropertyChanged.PropertyChanged{{TInstance}}(string, global::ZBase.Foundation.Mvvm.PropertyChangeEventListener{{TInstance}})\" />");
            p.PrintLine(GENERATED_CODE);
            p.PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine("public void PropertyChanged<TInstance>(string propertyName, global::ZBase.Foundation.Mvvm.PropertyChangeEventListener<TInstance> listener) where TInstance : class");
            p.OpenScope();
            {
                p.PrintLine("if (propertyName == null) throw new global::System.ArgumentNullException(nameof(propertyName));");
                p.PrintLine("if (listener == null) throw new global::System.ArgumentNullException(nameof(listener));");
                p.PrintEndLine();

                p.PrintLine("switch (propertyName)");
                p.OpenScope();
                {
                    foreach (var member in Members)
                    {
                        var propertyName = member.PropertyName;
                        var eventName = OnChangedEventName(member);

                        p.PrintLine($"case nameof(this.{propertyName}):");
                        p.OpenScope();
                        {
                            p.PrintLine($"this.{eventName} += listener.OnEvent;");
                            p.PrintLine($"listener.OnDetachAction = (listener) => this.{eventName} -= listener.OnEvent;");
                            p.PrintLine("break;");
                        }
                        p.CloseScope();
                        p.PrintEndLine();
                    }

                    foreach (var member in NotifyPropertyChangedForMap.Values)
                    {
                        var propertyName = member.Name;
                        var eventName = OnChangedEventName(member);

                        p.PrintLine($"case nameof(this.{propertyName}):");
                        p.OpenScope();
                        {
                            p.PrintLine($"this.{eventName} += listener.OnEvent;");
                            p.PrintLine($"listener.OnDetachAction = (listener) => this.{eventName} -= listener.OnEvent;");
                            p.PrintLine("break;");
                        }
                        p.CloseScope();
                        p.PrintEndLine();
                    }
                }
                p.CloseScope();
            }
            p.CloseScope();
            p.PrintEndLine();
        }

        private void WriteNotifyPropertyChangingAttributes(ref Printer p)
        {
            const string ATTRIBUTE = "[global::ZBase.Foundation.Mvvm.NotifyPropertyChangingAttribute(nameof({0}.{1}), typeof({2}))]";

            var className = Syntax.Identifier.Text;

            foreach (var member in Members)
            {
                var propertyName = member.PropertyName;
                var typeName = member.Member.Type.ToFullName();

                p.PrintLine(string.Format(ATTRIBUTE, className, propertyName, typeName));
            }
        }

        private void WriteNotifyPropertyChangedAttributes(ref Printer p)
        {
            const string ATTRIBUTE = "[global::ZBase.Foundation.Mvvm.NotifyPropertyChangedAttribute(nameof({0}.{1}), typeof({2}))]";

            var className = Syntax.Identifier.Text;
            var additionalProperties = new Dictionary<string, IPropertySymbol>();

            foreach (var member in Members)
            {
                var fieldName = member.Member.Name;
                var propertyName = member.PropertyName;
                var typeName = member.Member.Type.ToFullName();

                p.PrintLine(string.Format(ATTRIBUTE, className, propertyName, typeName));

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
                p.PrintLine(string.Format(ATTRIBUTE, className, property.Name, property.Type.ToFullName()));
            }
        }

        private void WriteUnionConverters(ref Printer p)
        {
            var types = new Dictionary<string, ITypeSymbol>();

            foreach (var member in Members)
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

            p.PrintLine(GENERATED_CODE);
            p.PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine("private static class UnionConverters");
            p.OpenScope();
            {
                foreach (var type in types.Values)
                {
                    var typeName = type.ToFullName();
                    var fieldName = type.ToValidIdentifier();

                    p.PrintLine(GENERATED_CODE);
                    p.PrintLine($"private static global::ZBase.Foundation.Unions.IUnionConverter<{typeName}> _{fieldName};");
                    p.PrintEndLine();
                }

                foreach (var type in types.Values)
                {
                    var typeName = type.ToFullName();
                    var fieldName = type.ToValidIdentifier();
                    var propertyName = GeneratorHelpers.ToTitleCase(fieldName.AsSpan());

                    p.PrintLine(GENERATED_CODE);
                    p.PrintLine(EXCLUDE_COVERAGE);
                    p.PrintLine($"public static global::ZBase.Foundation.Unions.IUnionConverter<{typeName}> {propertyName}");
                    p.OpenScope();
                    {
                        p.PrintLine($"get => _{fieldName} ??= global::ZBase.Foundation.Unions.UnionConverter.GetConverter<{typeName}>();");
                    }
                    p.CloseScope();
                    p.PrintEndLine();
                }
            }
            p.CloseScope();
            p.PrintEndLine();
        }

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
