using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    partial class RelayCommandDeclaration
    {
        private const string AGGRESSIVE_INLINING = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]";
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.ObservableObjectGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";

        public string WriteCode()
        {
            var scopePrinter = new SyntaxNodeScopePrinter(Printer.DefaultLarge, Syntax.Parent);
            var p = scopePrinter.printer;
            p = p.IncreasedIndent();

            p.PrintBeginLine()
                .Print("partial class ").Print(Syntax.Identifier.Text)
                .PrintEndLine();

            p.OpenScope();
            {

            }
            p.CloseScope();

            p = p.DecreasedIndent();
            return p.Result;
        }
    }
}
