// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using ZBase.Foundation.SourceGen;

using static ZBase.Foundation.Mvvm.SuppressionDescriptors;

namespace ZBase.Foundation.Mvvm.BinderSourceGen
{
    /// <summary>
    /// <para>
    /// A diagnostic suppressor to suppress CS0657 warnings for methods with [Binding] using a [field:] attribute list.
    /// </para>
    /// <para>
    /// That is, this diagnostic suppressor will suppress the following diagnostic:
    /// <code>
    /// public partial class MyBinder : IBinder
    /// {
    ///     [Binding]
    ///     [field: UnityEngine.SerializeField]
    ///     private void OnUpdate(int value) { }
    /// }
    /// </code>
    /// </para>
    /// </summary>
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public sealed class BindingAttributeWithPropertyTargetDiagnosticSuppressor : DiagnosticSuppressor
    {
        /// <inheritdoc/>
        public override ImmutableArray<SuppressionDescriptor> SupportedSuppressions => ImmutableArray.Create(PropertyAttributeListForObservablePropertyField);

        /// <inheritdoc/>
        public override void ReportSuppressions(SuppressionAnalysisContext context)
        {
            foreach (Diagnostic diagnostic in context.ReportedDiagnostics)
            {
                var syntaxNode = diagnostic.Location.SourceTree?.GetRoot(context.CancellationToken).FindNode(diagnostic.Location.SourceSpan);

                // Check that the target is effectively [field:] or [property:] over a method declaration, which is the case we're looking for
                if (syntaxNode is AttributeTargetSpecifierSyntax attributeTarget
                    && attributeTarget.Parent.Parent is MethodDeclarationSyntax methodDeclaration
                    && attributeTarget.Identifier.IsKind(SyntaxKind.FieldKeyword)
                )
                {
                    var semanticModel = context.GetSemanticModel(syntaxNode.SyntaxTree);

                    // Get the method symbol from the first variable declaration
                    var declaredSymbol = semanticModel.GetDeclaredSymbol(methodDeclaration, context.CancellationToken);

                    // Check if the method is using [RelayCommand], in which case we should suppress the warning
                    if (declaredSymbol is IMethodSymbol methodSymbol
                        && semanticModel.Compilation.GetTypeByMetadataName("ZBase.Foundation.Mvvm.ViewBinding.BindingAttribute") is INamedTypeSymbol bindingSymbol
                        && methodSymbol.HasAttributeWithType(bindingSymbol)
                    )
                    {
                        context.ReportSuppression(Suppression.Create(FieldAttributeListForBindingMethod, diagnostic));
                    }
                }
            }
        }
    }
}
