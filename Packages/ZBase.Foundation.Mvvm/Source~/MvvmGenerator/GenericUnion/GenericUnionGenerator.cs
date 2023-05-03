﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    [Generator]
    public class GenericUnionGenerator : IIncrementalGenerator
    {
        public const string INTERFACE = "global::ZBase.Foundation.Mvvm.Unions.IUnion<";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            var projectPathProvider = SourceGenHelpers.GetSourceGenConfigProvider(context);

            var candidateProvider = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (node, token) => GeneratorHelpers.IsStructSyntaxMatch(node, token),
                transform: static (syntaxContext, token) => GetStructSemanticMatch(syntaxContext, token)
            ).Where(static t => t is { });

            var combined = context.CompilationProvider
                .Combine(candidateProvider.Collect())
                .Combine(projectPathProvider);

            context.RegisterSourceOutput(combined, static (sourceProductionContext, source) => {
                GenerateOutput(
                    sourceProductionContext
                    , source.Left.Left
                    , source.Left.Right
                    , source.Right.projectPath
                    , source.Right.outputSourceGenFiles
                );
            });
        }

        public static StructRef GetStructSemanticMatch(
              GeneratorSyntaxContext context
            , CancellationToken token
        )
        {
            if (context.Node is not StructDeclarationSyntax structSyntax
                || structSyntax.BaseList == null
            )
            {
                return default;
            }

            var semanticModel = context.SemanticModel;

            foreach (var baseType in structSyntax.BaseList.Types)
            {
                var typeInfo = semanticModel.GetTypeInfo(baseType.Type, token);

                if (typeInfo.Type is INamedTypeSymbol interfaceSymbol)
                {
                    if (interfaceSymbol.IsGenericType
                       && interfaceSymbol.TypeParameters.Length == 1
                       && interfaceSymbol.ToFullName().StartsWith(INTERFACE)
                   )
                    {
                        return new StructRef {
                            Syntax = structSyntax,
                            TypeArgument = interfaceSymbol.TypeArguments[0],
                        };
                    }
                }

                if (TryGetMatchTypeArgument(typeInfo.Type.Interfaces, out var typeArgument)
                    || TryGetMatchTypeArgument(typeInfo.Type.AllInterfaces, out typeArgument)
                )
                {
                    return new StructRef {
                        Syntax = structSyntax,
                        TypeArgument = typeArgument,
                    };
                }
            }

            return default;

            static bool TryGetMatchTypeArgument(ImmutableArray<INamedTypeSymbol> interfaces, out ITypeSymbol typeArgument)
            {
                foreach (var interfaceSymbol in interfaces)
                {
                    if (interfaceSymbol.IsGenericType
                        && interfaceSymbol.TypeParameters.Length == 1
                        && interfaceSymbol.ToFullName().StartsWith(INTERFACE)
                    )
                    {
                        typeArgument = interfaceSymbol.TypeArguments[0];
                        return true;
                    }
                }

                typeArgument = default;
                return false;
            }
        }

        private static void GenerateOutput(
              SourceProductionContext context
            , Compilation compilation
            , ImmutableArray<StructRef> candidates
            , string projectPath
            , bool outputSourceGenFiles
        )
        {
            if (candidates.Length < 1)
            {
                return;
            }

            context.CancellationToken.ThrowIfCancellationRequested();

            SourceGenHelpers.ProjectPath = projectPath;

            var declaration = new GenericUnionDeclaration(candidates, compilation, context.CancellationToken);

            declaration.GenerateUnionTypes(
                  context
                , compilation
                , outputSourceGenFiles
            );

            declaration.GenerateStaticClass(
                  context
                , compilation
                , outputSourceGenFiles
            );

            declaration.GenerateRedundantTypes(
                  context
                , compilation
                , outputSourceGenFiles
            );
        }

        private static readonly DiagnosticDescriptor s_errorDescriptor
            = new("SG_GENERIC_UNION_01"
                , "Generic Union Generator Error"
                , "This error indicates a bug in the Generic Union source generators. Error message: '{0}'."
                , "ZBase.Foundation.Mvvm.Unions.IUnion<T>"
                , DiagnosticSeverity.Error
                , isEnabledByDefault: true
                , description: ""
            );
    }
}