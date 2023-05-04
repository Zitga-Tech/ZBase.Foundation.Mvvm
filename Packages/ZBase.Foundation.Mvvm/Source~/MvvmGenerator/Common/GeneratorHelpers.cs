using System;
using System.Collections.Immutable;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm
{
    public static class GeneratorHelpers
    {
        private const string GENERATED_CODE = "[global::System.CodeDom.Compiler.GeneratedCode(\"ZBase.Foundation.Mvvm.InternalUnionGenerator\", \"1.0.0\")]";
        private const string EXCLUDE_COVERAGE = "[global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]";

        public const string FIELD_PREFIX_UNDERSCORE = "_";
        public const string FIELD_PREFIX_M_UNDERSCORE = "m_";

        public static bool IsValidCompilation(this Compilation compilation)
            => string.Equals(compilation?.AssemblyName, "ZBase.Foundation.Mvvm") == false;

        public static bool IsClassSyntaxMatch(SyntaxNode syntaxNode , CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return syntaxNode is ClassDeclarationSyntax classSyntax
                && classSyntax.BaseList != null
                && classSyntax.BaseList.Types.Count > 0;
        }

        public static bool IsStructSyntaxMatch(SyntaxNode syntaxNode, CancellationToken token)
        {
            token.ThrowIfCancellationRequested();

            return syntaxNode is StructDeclarationSyntax structSyntax
                && structSyntax.BaseList != null
                && structSyntax.BaseList.Types.Count > 0;
        }

        public static bool IsClassSyntaxMatchByAttribute(
              SyntaxNode syntaxNode
            , CancellationToken token
            , SyntaxKind syntaxKind
            , string attributeNamespace
            , string attributeName
        )
        {
            token.ThrowIfCancellationRequested();
            
            if (syntaxNode is not ClassDeclarationSyntax classSyntax
                || classSyntax.BaseList == null
                || classSyntax.BaseList.Types.Count < 1
            )
            {
                return false;
            }
            
            var members = classSyntax.Members;

            foreach (var member in members)
            {
                if (member.Kind() == syntaxKind
                    && member.HasAttributeCandidate(attributeNamespace, attributeName)
                )
                {
                    return true;
                }
            }

            return false;
        }

        public static ClassDeclarationSyntax GetClassSemanticMatch(
              GeneratorSyntaxContext context
            , CancellationToken token
            , string interfaceName
        )
        {
            token.ThrowIfCancellationRequested();

            if (context.Node is not ClassDeclarationSyntax classSyntax
                || classSyntax.BaseList == null
                || classSyntax.DoesSemanticMatch(interfaceName, context.SemanticModel, token) == false
            )
            {
                return null;
            }

            return classSyntax;
        }

        public static bool DoesSemanticMatch(
              this ClassDeclarationSyntax classSyntax
            , string interfaceName
            , SemanticModel semanticModel
            , CancellationToken token
        )
        {
            foreach (var baseType in classSyntax.BaseList.Types)
            {
                var typeInfo = semanticModel.GetTypeInfo(baseType.Type, token);

                if (typeInfo.Type.ToFullName() == interfaceName)
                {
                    return true;
                }

                if (typeInfo.Type.ImplementsInterface(interfaceName))
                {
                    return true;
                }

                if (IsMatch(typeInfo.Type.Interfaces, interfaceName)
                    || IsMatch(typeInfo.Type.AllInterfaces, interfaceName)
                )
                {
                    return true;
                }
            }

            return false;

            static bool IsMatch(ImmutableArray<INamedTypeSymbol> interfaces, string interfaceName)
            {
                foreach (var interfaceSymbol in interfaces)
                {
                    if (interfaceSymbol.ToFullName() == interfaceName)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public static string ToPropertyName(this IFieldSymbol field)
        {
            var nameSpan = field.Name.AsSpan();
            var prefix = FIELD_PREFIX_UNDERSCORE.AsSpan();

            if (nameSpan.StartsWith(prefix))
            {
                return ToTitleCase(nameSpan.Slice(1));
            }

            prefix = FIELD_PREFIX_M_UNDERSCORE.AsSpan();

            if (nameSpan.StartsWith(prefix))
            {
                return ToTitleCase(nameSpan.Slice(2));
            }

            return ToTitleCase(nameSpan);
        }

        public static string ToTitleCase(in ReadOnlySpan<char> value)
        {
            return $"{char.ToUpper(value[0])}{value.Slice(1).ToString()}";
        }

        public static string GetKeyword(this Accessibility accessibility)
            => accessibility switch {
                Accessibility.Public => "public",
                Accessibility.Private => "private",
                Accessibility.Protected => "protected",
                Accessibility.Internal => "internal",
                Accessibility.ProtectedOrInternal => "protected internal",
                Accessibility.ProtectedAndInternal => "private protected",
                _ => ""
            };

        public static Printer WritePreserveAttributeClass(Printer p)
        {
            p.PrintLine(GENERATED_CODE).PrintLine(EXCLUDE_COVERAGE);
            p.PrintLine("[global::System.AttributeUsage(");
            p = p.IncreasedIndent();
            {
                p.PrintLine("  global::System.AttributeTargets.Assembly");
                p.PrintLine("| global::System.AttributeTargets.Class");
                p.PrintLine("| global::System.AttributeTargets.Struct");
                p.PrintLine("| global::System.AttributeTargets.Enum");
                p.PrintLine("| global::System.AttributeTargets.Constructor");
                p.PrintLine("| global::System.AttributeTargets.Method");
                p.PrintLine("| global::System.AttributeTargets.Property");
                p.PrintLine("| global::System.AttributeTargets.Field");
                p.PrintLine("| global::System.AttributeTargets.Event");
                p.PrintLine("| global::System.AttributeTargets.Interface");
                p.PrintLine("| global::System.AttributeTargets.Delegate");
                p.PrintLine(", Inherited = false");
            }
            p = p.DecreasedIndent();
            p.PrintLine(")]");
            p.PrintLine("public sealed class PreserveAttribute : global::System.Attribute { }");
            return p;
        }
    }
}