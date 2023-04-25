using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    public static class GeneratorHelpers
    {
        public const string NAMESPACE = "ZBase.Foundation.Mvvm";
        public const string IOBSERVABLE_OBJECT_NAME = "IObservableObject";
        public const string IOBSERVABLE_OBJECT_INTERFACE = "global::ZBase.Foundation.Mvvm.IObservableObject";

        public static bool IsSyntaxMatch(
              SyntaxNode syntaxNode
            , CancellationToken token
            , SyntaxKind syntaxKind
            , string attributeName
        )
        {
            token.ThrowIfCancellationRequested();

            if (syntaxNode is not ClassDeclarationSyntax classSyntax
                || classSyntax.BaseList == null
            )
            {
                return false;
            }

            var implementInterface = false;

            foreach (var baseType in classSyntax.BaseList.Types)
            {
                if (baseType.Type is IdentifierNameSyntax { Identifier: { ValueText: IOBSERVABLE_OBJECT_NAME } })
                {
                    implementInterface = true;
                    break;
                }
            }

            if (implementInterface == false)
            {
                return false;
            }

            var members = classSyntax.Members;

            foreach (var member in members)
            {
                if (member.Kind() == syntaxKind
                    && member.HasAttributeCandidate(NAMESPACE, attributeName)
                )
                {
                    return true;
                }
            }

            return false;
        }

        public static ClassDeclarationSyntax GetSemanticMatch(
              GeneratorSyntaxContext syntaxContext
            , CancellationToken token
        )
        {
            token.ThrowIfCancellationRequested();

            if (syntaxContext.Node is not ClassDeclarationSyntax classSyntax
                || classSyntax.BaseList == null
            )
            {
                return null;
            }

            var semanticModel = syntaxContext.SemanticModel;

            foreach (var baseType in classSyntax.BaseList.Types)
            {
                var typeInfo = semanticModel.GetTypeInfo(baseType.Type, token);

                if (typeInfo.Type.ToFullName().StartsWith(IOBSERVABLE_OBJECT_INTERFACE))
                {
                    return classSyntax;
                }
            }

            return null;
        }
    }
}