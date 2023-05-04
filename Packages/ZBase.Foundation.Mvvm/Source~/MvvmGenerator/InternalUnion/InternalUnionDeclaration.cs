using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    public partial class InternalUnionDeclaration
    {
        public const string COMPONENT_MODEL_NAMESPACE = "ZBase.Foundation.Mvvm.ComponentModel";
        public const string OBSERVABLE_PROPERTY = "ObservableProperty";
        public const string OBSERVABLE_PROPERTY_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.ComponentModel.ObservablePropertyAttribute";
        public const string RELAY_COMMAND_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.Input.RelayCommandAttribute";

        public ImmutableArray<TypeRef> TypeRefs { get; }

        public InternalUnionDeclaration(
              ImmutableArray<ClassDeclarationSyntax> candidates
            , Compilation compilation
            , CancellationToken token
        )
        {
            var filtered = new Dictionary<string, TypeRef>();

            foreach (var candidate in candidates)
            {
                var syntaxTree = candidate.SyntaxTree;
                var semanticModel = compilation.GetSemanticModel(syntaxTree);
                var memberSyntaxes = candidate.Members;

                foreach (var memberSyntax in memberSyntaxes)
                {
                    if (memberSyntax is FieldDeclarationSyntax fieldSyntax)
                    {
                        if (fieldSyntax.HasAttributeCandidate(COMPONENT_MODEL_NAMESPACE, OBSERVABLE_PROPERTY))
                        {
                            var syntax = fieldSyntax.Declaration.Type;
                            var typeInfo = semanticModel.GetTypeInfo(syntax, token);
                            var symbol = typeInfo.Type;
                            var typeName = symbol.ToFullName();

                            if (typeName.ToUnionType().IsNativeUnionType() == false
                                && symbol.IsValueType == true
                                && filtered.ContainsKey(typeName) == false
                            )
                            {
                                filtered[typeName] = new TypeRef {
                                    Symbol = symbol,
                                    Syntax = fieldSyntax.Declaration.Type,
                                };
                            }
                        }

                        continue;
                    }

                    if (memberSyntax is MethodDeclarationSyntax methodSyntax)
                    {
                        var info = semanticModel.GetDeclaredSymbol(methodSyntax, token);

                        if (info is IMethodSymbol method
                            && method.Parameters.Length == 1
                            && method.HasAttribute(RELAY_COMMAND_ATTRIBUTE)
                        )
                        {
                            var symbol = method.Parameters[0].Type;
                            var typeName = symbol.ToFullName();

                            if (typeName.ToUnionType().IsNativeUnionType() == false
                                && symbol.IsValueType == true
                                && filtered.ContainsKey(typeName) == false
                            )
                            {
                                filtered[typeName] = new TypeRef {
                                    Symbol = symbol,
                                    Syntax = methodSyntax.ReturnType,
                                };
                            }
                        }

                        continue;
                    }
                }
            }

            using var typeRefs = ImmutableArrayBuilder<TypeRef>.Rent();
            typeRefs.AddRange(filtered.Values);

            TypeRefs = typeRefs.ToImmutable();
        }

        public class TypeRef
        {
            public TypeSyntax Syntax { get; set; }

            public ITypeSymbol Symbol { get; set; }
        }
    }
}
