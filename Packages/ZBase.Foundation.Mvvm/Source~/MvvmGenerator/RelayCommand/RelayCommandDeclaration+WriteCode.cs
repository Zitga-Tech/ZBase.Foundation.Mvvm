using Microsoft.CodeAnalysis;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    partial class RelayCommandDeclaration
    {
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.RelayCommandGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";

        public string WriteCode()
        {
            var scopePrinter = new SyntaxNodeScopePrinter(Printer.DefaultLarge, Syntax.Parent);
            var p = scopePrinter.printer;
            p = p.IncreasedIndent();

            p.PrintBeginLine();
            p.Print("partial class ").Print(Syntax.Identifier.Text);
            p.PrintEndLine();

            p.OpenScope();
            {
                WriteFields(ref p);
                WriteProperties(ref p);
            }
            p.CloseScope();

            p = p.DecreasedIndent();
            return p.Result;
        }

        private void WriteFields(ref Printer p)
        {
            foreach (var member in Members)
            {
                var propertyName = CommandPropertyName(member.Member);
                var fieldName = CommandFieldName(member.Member);
                var typeName = CommandTypeName(member.Member);

                p.PrintLine($"/// <summary>The backing field for <see cref=\"{propertyName}\"/>.</summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private {typeName} {fieldName};");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteProperties(ref Printer p)
        {
            foreach (var member in Members)
            {
                var method = member.Member;
                var propertyName = CommandPropertyName(method);
                var fieldName = CommandFieldName(method);
                var typeName = CommandTypeName(method);
                var interfaceName = CommandInterfaceName(method);
                var interfaceNameComment = CommandInterfaceNameComment(method);
                var delegateName = CommandDelegateTypeName(method);
                var canExecuteArg = CanExecuteMethodArg(member.CanExecuteMethod);

                p.PrintLine($"/// <summary>Gets an <see cref=\"{interfaceNameComment}\"/> instance wrapping <see cref=\"{method.Name}\"/>.</summary>");
                p.PrintLine(GENERATED_CODE);
                p.PrintLine(EXCLUDE_COVERAGE);
                p.PrintLine($"public {interfaceName} {propertyName}");
                p.OpenScope();
                {
                    p.PrintLine("get");
                    p.OpenScope();
                    {
                        p.PrintLine($"if ({fieldName} == null)");
                        p = p.IncreasedIndent();
                        p.PrintLine($"{fieldName} = new {typeName}(new {delegateName}({method.Name}){canExecuteArg});");
                        p = p.DecreasedIndent();
                        p.PrintEndLine();

                        p.PrintLine($"return {fieldName};");
                    }
                    p.CloseScope();
                }
                p.CloseScope();
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private string CommandFieldName(IMethodSymbol method)
            => $"_command{method.Name}";

        private string CommandPropertyName(IMethodSymbol method)
            => $"{method.Name}Command";

        private string CommandTypeName(IMethodSymbol method)
        {
            if (method.Parameters.Length < 1)
                return "global::ZBase.Foundation.Mvvm.RelayCommand";

            var param = method.Parameters[0];
            return $"global::ZBase.Foundation.Mvvm.RelayCommand<{param.Type.ToFullName()}>";
        }

        private string CommandInterfaceName(IMethodSymbol method)
        {
            if (method.Parameters.Length < 1)
                return "global::ZBase.Foundation.Mvvm.IRelayCommand";

            var param = method.Parameters[0];
            return $"global::ZBase.Foundation.Mvvm.IRelayCommand<{param.Type.ToFullName()}>";
        }

        private string CommandInterfaceNameComment(IMethodSymbol method)
        {
            if (method.Parameters.Length < 1)
                return "global::ZBase.Foundation.Mvvm.IRelayCommand";

            var param = method.Parameters[0];
            return $"global::ZBase.Foundation.Mvvm.IRelayCommand{{{param.Type.ToFullName()}}}";
        }

        private string CommandDelegateTypeName(IMethodSymbol method)
        {
            if (method.Parameters.Length < 1)
                return "global::System.Action";

            var param = method.Parameters[0];
            return $"global::System.Action<{param.Type.ToFullName()}>";
        }

        private string CanExecuteMethodArg(IMethodSymbol method)
        {
            if (method == null)
                return string.Empty;

            if (method.Parameters.Length < 1)
                return $", {method.Name}()";

            return $", {method.Name}";
        }
    }
}
