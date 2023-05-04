using Microsoft.CodeAnalysis;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm.RelayCommandSourceGen
{
    partial class RelayCommandDeclaration
    {
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.RelayCommandGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";
        private const string RELAY_COMMAND = "[global::ZBase.Foundation.Mvvm.Input.GeneratedRelayCommand]";

        public string WriteCode()
        {
            var scopePrinter = new SyntaxNodeScopePrinter(Printer.DefaultLarge, Syntax.Parent);
            var p = scopePrinter.printer;

            p.PrintLine("#pragma warning disable");
            p.PrintEndLine();

            p = p.IncreasedIndent();

            WriteRelayCommandInfoAttributes(ref p);

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

        private void WriteRelayCommandInfoAttributes(ref Printer p)
        {
            var className = Syntax.Identifier.Text;

            foreach (var member in MemberRefs)
            {
                var commandName = CommandPropertyName(member.Member);

                if (member.Member.Parameters.Length > 0)
                {
                    var param = member.Member.Parameters[0];
                    var paramType = param.Type.ToFullName();

                    p.PrintLine($"[global::ZBase.Foundation.Mvvm.Input.RelayCommandInfo(nameof({className}.{commandName}), typeof({paramType}))]");
                }
                else
                {
                    p.PrintLine($"[global::ZBase.Foundation.Mvvm.Input.RelayCommandInfo(nameof({className}.{commandName}))]");
                }
            }
        }

        private void WriteFields(ref Printer p)
        {
            foreach (var member in MemberRefs)
            {
                var propertyName = CommandPropertyName(member.Member);
                var fieldName = CommandFieldName(member.Member);
                var typeName = CommandTypeName(member.Member);

                p.PrintLine($"/// <summary>The backing field for <see cref=\"{propertyName}\"/>.</summary>");

                foreach (var attribute in member.ForwardedFieldAttributes)
                {
                    p.PrintLine($"[{attribute.GetSyntax().ToFullString()}]");
                }

                p.PrintLine(GENERATED_CODE);
                p.PrintLine($"private {typeName} {fieldName};");
                p.PrintEndLine();
            }

            p.PrintEndLine();
        }

        private void WriteProperties(ref Printer p)
        {
            foreach (var member in MemberRefs)
            {
                var method = member.Member;
                var propertyName = CommandPropertyName(method);
                var fieldName = CommandFieldName(method);
                var typeName = CommandTypeName(method);
                var interfaceName = CommandInterfaceName(method);
                var interfaceNameComment = CommandInterfaceNameComment(method);
                var canExecuteArg = CanExecuteMethodArg(member.CanExecuteMethod);

                p.PrintLine($"/// <summary>Gets an <see cref=\"{interfaceNameComment}\"/> instance wrapping <see cref=\"{method.Name}\"/>.</summary>");

                foreach (var attribute in member.ForwardedPropertyAttributes)
                {
                    p.PrintLine($"[{attribute.GetSyntax().ToFullString()}]");
                }

                p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE).PrintLine(RELAY_COMMAND);
                p.PrintLine($"public {interfaceName} {propertyName}");
                p.OpenScope();
                {
                    p.PrintLine("get");
                    p.OpenScope();
                    {
                        p.PrintLine($"if (this.{fieldName} == null)");
                        p = p.IncreasedIndent();
                        p.PrintLine($"this.{fieldName} = new {typeName}({method.Name}{canExecuteArg});");
                        p = p.DecreasedIndent();
                        p.PrintEndLine();

                        p.PrintLine($"return this.{fieldName};");
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
                return "global::ZBase.Foundation.Mvvm.Input.RelayCommand";

            var param = method.Parameters[0];
            return $"global::ZBase.Foundation.Mvvm.Input.RelayCommand<{param.Type.ToFullName()}>";
        }

        private string CommandInterfaceName(IMethodSymbol method)
        {
            if (method.Parameters.Length < 1)
                return "global::ZBase.Foundation.Mvvm.Input.IRelayCommand";

            var param = method.Parameters[0];
            return $"global::ZBase.Foundation.Mvvm.Input.IRelayCommand<{param.Type.ToFullName()}>";
        }

        private string CommandInterfaceNameComment(IMethodSymbol method)
        {
            if (method.Parameters.Length < 1)
                return "global::ZBase.Foundation.Mvvm.Input.IRelayCommand";

            var param = method.Parameters[0];
            return $"global::ZBase.Foundation.Mvvm.Input.IRelayCommand{{{param.Type.ToFullName()}}}";
        }

        private string CanExecuteMethodArg(IMethodSymbol method)
        {
            if (method == null)
                return string.Empty;

            if (method.Parameters.Length < 1)
                return $", _ => {method.Name}()";

            return $", {method.Name}";
        }
    }
}
