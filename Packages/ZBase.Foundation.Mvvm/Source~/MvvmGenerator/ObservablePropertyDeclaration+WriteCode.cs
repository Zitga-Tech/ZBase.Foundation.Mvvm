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

            WritePropertyNamesClass(ref p);

            p.PrintBeginLine();
            p.Print("partial class ").Print(Syntax.Identifier.Text);
            p.PrintEndLine();

            p = p.IncreasedIndent();
            p.PrintLine(": INotifyPropertyChanging, INotifyPropertyChanged");
            p = p.DecreasedIndent();

            p.OpenScope();
            {
                WriteEvents(ref p);
                WriteProperties(ref p);
                WritePartialMethods(ref p);
                WritePropertyChangingMethod(ref p);
                WritePropertyChangedMethod(ref p);
            }
            p.CloseScope();

            p = p.DecreasedIndent();
            return p.Result;
        }

        private void WritePropertyNamesClass(ref Printer p)
        {
            var accessKeyword = Symbol.DeclaredAccessibility.ToKeyword();

            p.PrintLine(GENERATED_CODE);
            p.PrintLine(EXCLUDE_COVERAGE);
            p.PrintBeginLine();

            if (string.IsNullOrEmpty(accessKeyword) == false)
            {
                p.Print(accessKeyword).Print(" ");
            }

            p.Print("static partial class ").Print(GetStaticClassName());
            p.PrintEndLine();

            p.OpenScope();
            {
                foreach (var member in Members)
                {
                    var propertyName = member.PropertyName;

                    p.PrintLine($"/// <inheritdoc cref=\"{FullyQualifiedName}.{propertyName}\" />");
                    p.PrintLine(GENERATED_CODE);
                    p.PrintLine($"public const string {propertyName} = nameof({propertyName});");
                    p.PrintEndLine();
                }

                foreach (var member in NotifyPropertyChangedForMap.Values)
                {
                    var propertyName = member.Name;

                    p.PrintLine($"/// <inheritdoc cref=\"{FullyQualifiedName}.{propertyName}\" />");
                    p.PrintLine(GENERATED_CODE);
                    p.PrintLine($"public const string {propertyName} = nameof({propertyName});");
                    p.PrintEndLine();
                }
            }
            p.CloseScope();
            p.PrintEndLine();
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
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler {OnChangingEventName(member)};");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler {OnChangedEventName(member)};");
                p.PrintEndLine();
            }
        }

        private void WriteProperties(ref Printer p)
        {
            var staticClassName = GetStaticClassName();

            foreach (var member in Members)
            {
                var fieldName = member.Member.Name;
                var propertyName = member.PropertyName;
                var typeName = member.Member.Type.ToFullName();
                var argsName = OnChangedArgsName(member);

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
                            p.PrintLine($"var {argsName} = new PropertyChangeEventArgs(this, {staticClassName}.{propertyName}, value);");
                            p.PrintLine($"this.{OnChangingEventName(member)}?.Invoke({argsName});");
                            p.PrintLine($"this.{fieldName} = value;");
                            p.PrintLine($"{OnChangedMethodName(member)}(value);");
                            p.PrintLine($"this.{OnChangedEventName(member)}?.Invoke({argsName});");

                            if (NotifyPropertyChangedForMap.TryGetValue(fieldName, out var property))
                            {
                                var otherArgsName = OnChangedArgsName(property);
                                p.PrintEndLine();
                                p.PrintLine($"{OnChangedMethodName(property)}(this.{propertyName});");
                                p.PrintLine($"var {otherArgsName} = new PropertyChangeEventArgs(this, {staticClassName}.{propertyName}, this.{propertyName});");
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
            var staticClassName = GetStaticClassName();

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

                        p.PrintLine($"case {staticClassName}.{propertyName}:");
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
            var staticClassName = GetStaticClassName();

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

                        p.PrintLine($"case {staticClassName}.{propertyName}:");
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

                        p.PrintLine($"case {staticClassName}.{propertyName}:");
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

        private string GetStaticClassName()
            => $"{Syntax.Identifier.Text}PropertyNames";

        private string OnChangingEventName(MemberRef member)
            => $"_onChanging{member.PropertyName}";

        private string OnChangedEventName(MemberRef member)
            => $"_onChanged{member.PropertyName}";

        private string OnChangingEventName(ISymbol member)
            => $"_onChanging{member.Name}";

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
