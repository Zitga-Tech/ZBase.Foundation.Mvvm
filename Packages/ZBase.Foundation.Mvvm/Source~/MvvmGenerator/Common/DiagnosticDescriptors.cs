// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.CodeAnalysis;

#pragma warning disable IDE0090 // Use 'new DiagnosticDescriptor(...)'
#pragma warning disable IDE1006 // Naming Styles
#pragma warning disable RS2008 // Enable analyzer release tracking

namespace ZBase.Foundation.Mvvm
{
    /// <summary>
    /// A container for all <see cref="DiagnosticDescriptor"/> instances for errors reported by analyzers in this project.
    /// </summary>
    internal static class DiagnosticDescriptors
    {
        /// <summary>
        /// The diagnostic id for <see cref="FieldReferenceForObservablePropertyFieldWarning"/>.
        /// </summary>
        public const string FieldReferenceForObservablePropertyFieldId = "MVVMTK0034";

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when a field with <c>[ObservableProperty]</c> is being directly referenced.
        /// <para>
        /// Format: <c>"The field {0} is annotated with [ObservableProperty] and should not be directly referenced (use the generated property instead)"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor FieldReferenceForObservablePropertyFieldWarning = new DiagnosticDescriptor(
        id: FieldReferenceForObservablePropertyFieldId,
        title: "Direct field reference to [ObservableProperty] backing field",
        messageFormat: "The field {0} is annotated with [ObservableProperty] and should not be directly referenced (use the generated property instead)",
        category: "ObservablePropertyGenerator",
        defaultSeverity: DiagnosticSeverity.Warning,
        isEnabledByDefault: true,
        description: "Fields with [ObservableProperty] should not be directly referenced, and the generated properties should be used instead.",
        helpLinkUri: "https://aka.ms/mvvmtoolkit/errors/mvvmtk0034");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when a field with <c>[ObservableProperty]</c> is using an invalid attribute targeting the property.
        /// <para>
        /// Format: <c>"The field {0} annotated with [ObservableProperty] is using attribute "{1}" which was not recognized as a valid type (are you missing a using directive?)"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor InvalidPropertyTargetedAttributeOnObservablePropertyField = new DiagnosticDescriptor(
        id: "MVVMTK0035",
        title: "Invalid property targeted attribute type",
        messageFormat: "The field {0} annotated with [ObservableProperty] is using attribute \"{1}\" which was not recognized as a valid type (are you missing a using directive?)",
        category: "ObservablePropertyGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "All attributes targeting the generated property for a field annotated with [ObservableProperty] must correctly be resolved to valid types.",
        helpLinkUri: "https://aka.ms/mvvmtoolkit/errors/mvvmtk0035");

        /// <summary>
        /// Gets a <see cref="DiagnosticDescriptor"/> indicating when a method with <c>[RelayCommand]</c> is using an invalid attribute targeting the field or property.
        /// <para>
        /// Format: <c>"The method {0} annotated with [RelayCommand] is using attribute "{1}" which was not recognized as a valid type (are you missing a using directive?)"</c>.
        /// </para>
        /// </summary>
        public static readonly DiagnosticDescriptor InvalidFieldOrPropertyTargetedAttributeOnRelayCommandMethod = new DiagnosticDescriptor(
        id: "MVVMTK0036",
        title: "Invalid field targeted attribute type",
        messageFormat: "The method {0} annotated with [RelayCommand] is using attribute \"{1}\" which was not recognized as a valid type (are you missing a using directive?)",
        category: "RelayCommandGenerator",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "All attributes targeting the generated field or property for a method annotated with [RelayCommand] must correctly be resolved to valid types.",
        helpLinkUri: "https://aka.ms/mvvmtoolkit/errors/mvvmtk0036");
    }
}
