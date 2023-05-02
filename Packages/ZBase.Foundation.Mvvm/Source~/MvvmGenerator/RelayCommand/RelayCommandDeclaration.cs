using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    public partial class RelayCommandDeclaration
    {
        public const string RELAY_COMMAND_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.RelayCommandAttribute";

        public ClassDeclarationSyntax Syntax { get; }

        public string FullyQualifiedName { get; private set; }

        public bool IsValid { get; }

        public List<MemberRef> Members { get; }

        public RelayCommandDeclaration(ClassDeclarationSyntax candidate, SemanticModel semanticModel, CancellationToken token)
        {
            var typeSymbol = semanticModel.GetDeclaredSymbol(candidate, token);

            Syntax = candidate;
            FullyQualifiedName = typeSymbol.ToFullName();
            Members = new List<MemberRef>();

            var members = typeSymbol.GetMembers();
            var methodMap = new Dictionary<string, IMethodSymbol>();
            var methodCandidates = new List<(IMethodSymbol, string)>();

            foreach (var member in members)
            {
                if (member is not IMethodSymbol method || method.Parameters.Length > 1)
                {
                    continue;
                }

                var relayCommandAttrib = method.GetAttribute(RELAY_COMMAND_ATTRIBUTE);

                if (relayCommandAttrib != null)
                {
                    if (relayCommandAttrib.NamedArguments.Length > 0)
                    {
                        foreach (var kv in relayCommandAttrib.NamedArguments)
                        {
                            if (kv.Key == "CanExecute"
                                && kv.Value.Value is string canExecuteMethodName
                                && canExecuteMethodName != method.Name
                            )
                            {
                                methodCandidates.Add((method, canExecuteMethodName));
                            }
                        }
                    }
                    else
                    {
                        Members.Add(new MemberRef { Member = method });
                    }
                }

                methodMap[method.Name] = method;
            }

            var paramSet = new HashSet<string>();

            foreach (var (method, canExecuteName) in methodCandidates)
            {
                paramSet.Clear();

                if (methodMap.TryGetValue(canExecuteName, out var canExecuteMethod) == false
                    || canExecuteMethod.ReturnType.SpecialType != SpecialType.System_Boolean
                )
                {
                    continue;
                }

                var isValid = true;

                if (canExecuteMethod.Parameters.Length != 0)
                {
                    if (method.Parameters.Length != canExecuteMethod.Parameters.Length)
                    {
                        continue;
                    }

                    foreach (var param in method.Parameters)
                    {
                        paramSet.Add(param.Type.ToFullName());
                    }

                    foreach (var param in canExecuteMethod.Parameters)
                    {
                        if (paramSet.Contains(param.Type.ToFullName()) == false)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }

                if (isValid)
                {
                    Members.Add(new MemberRef {
                        Member = method,
                        CanExecuteMethod = canExecuteMethod,
                    });
                }
            }

            IsValid = Members.Count > 0;
        }

        public class MemberRef
        {
            public IMethodSymbol Member { get; set; }

            public IMethodSymbol CanExecuteMethod { get; set; }
        }
    }
}
