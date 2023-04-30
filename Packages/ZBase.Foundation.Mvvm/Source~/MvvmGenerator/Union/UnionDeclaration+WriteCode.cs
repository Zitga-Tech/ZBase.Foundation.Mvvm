using Microsoft.CodeAnalysis;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    partial class UnionDeclaration
    {
        public string WriteCode()
        {
            var scopePrinter = new SyntaxNodeScopePrinter(Printer.DefaultLarge, Syntax.Parent);
            var p = scopePrinter.printer;
            p = p.IncreasedIndent();

            p = p.DecreasedIndent();
            return p.Result;
        }
    }
}
