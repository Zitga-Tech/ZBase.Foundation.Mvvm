using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    public partial class RelayCommandDeclaration
    {
        public const string RELAY_COMMAND_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.Input.RelayCommandAttribute";

        public ClassDeclarationSyntax Syntax { get; }

        public ITypeSymbol Symbol { get; }

        public string FullyQualifiedName { get; private set; }

        public bool IsValid { get; }

        public ImmutableArray<MemberRef> MemberRefs { get; }

        public RelayCommandDeclaration(ClassDeclarationSyntax candidate, SemanticModel semanticModel, CancellationToken token)
        {
            using var memberRefs = ImmutableArrayBuilder<MemberRef>.Rent();
            using var diagnosticBuilder = ImmutableArrayBuilder<DiagnosticInfo>.Rent();

            Syntax = candidate;
            Symbol = semanticModel.GetDeclaredSymbol(candidate, token);
            FullyQualifiedName = Symbol.ToFullName();

            var members = Symbol.GetMembers();
            var methodMap = new Dictionary<string, IMethodSymbol>();
            var methodCandidates = new List<(IMethodSymbol, string)>();

            foreach (var member in members)
            {
                if (member is not IMethodSymbol method
                    || method.Parameters.Length > 1
                )
                {
                    continue;
                }

                if (method.Parameters.Length == 1)
                {
                    var parameter = method.Parameters[0];

                    if (parameter.RefKind is not (RefKind.None or RefKind.In))
                    {
                        continue;
                    }
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
                        memberRefs.Add(new MemberRef { Member = method });
                    }
                }

                methodMap[method.Name] = method;
            }

            var filtered = new HashSet<string>();

            foreach (var (method, canExecuteName) in methodCandidates)
            {
                filtered.Clear();

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
                        filtered.Add(param.Type.ToFullName());
                    }

                    foreach (var param in canExecuteMethod.Parameters)
                    {
                        if (filtered.Contains(param.Type.ToFullName()) == false)
                        {
                            isValid = false;
                            break;
                        }
                    }
                }

                if (isValid)
                {
                    memberRefs.Add(new MemberRef {
                        Member = method,
                        CanExecuteMethod = canExecuteMethod,
                    });
                }
            }

            MemberRefs = memberRefs.ToImmutable();
            IsValid = MemberRefs.Length > 0;

            foreach (var memberRef in MemberRefs)
            {
                memberRef.Member.GatherForwardedAttributes(
                      semanticModel
                    , token
                    , diagnosticBuilder
                    , out var fieldAttributes
                    , out var propertyAttributes
                    , DiagnosticDescriptors.InvalidFieldOrPropertyTargetedAttributeOnRelayCommandMethod
                );

                memberRef.ForwardedFieldAttributes = fieldAttributes;
                memberRef.ForwardedPropertyAttributes = propertyAttributes;
            }
        }

        public class MemberRef
        {
            public IMethodSymbol Member { get; set; }

            public IMethodSymbol CanExecuteMethod { get; set; }

            public ImmutableArray<AttributeInfo> ForwardedFieldAttributes { get; set; }

            public ImmutableArray<AttributeInfo> ForwardedPropertyAttributes { get; set; }
        }
    }
}
