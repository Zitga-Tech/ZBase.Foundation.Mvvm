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

            p.Print("static partial class ").Print(Syntax.Identifier.Text).Print("PropertyNames");
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
                var propertyName = member.PropertyName;

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler _onChanging{propertyName};");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler _onChanged{propertyName};");
                p.PrintEndLine();
            }

            foreach (var member in NotifyPropertyChangedForMap.Values)
            {
                var propertyName = member.Name;

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler _onChanging{propertyName};");
                p.PrintEndLine();

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler _onChanged{propertyName};");
                p.PrintEndLine();
            }
        }
    }
}
