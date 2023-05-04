using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.Collections.Immutable;
using ZBase.Foundation.SourceGen;

namespace ZBase.Foundation.Mvvm.InternalUnionSourceGen
{
    public partial class InternalUnionDeclaration
    {
        public const string COMPONENT_MODEL_NAMESPACE = "ZBase.Foundation.Mvvm.ComponentModel";
        public const string OBSERVABLE_PROPERTY = "ObservableProperty";
        public const string OBSERVABLE_PROPERTY_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.ComponentModel.ObservablePropertyAttribute";
        public const string RELAY_COMMAND_ATTRIBUTE = "global::ZBase.Foundation.Mvvm.Input.RelayCommandAttribute";

        public ImmutableArray<TypeRef> TypeRefs { get; }

        public InternalUnionDeclaration(ImmutableArray<TypeRef> candidates)
        {
            var filtered = new Dictionary<string, TypeRef>();

            foreach (var candidate in candidates)
            {
                var symbol = candidate.Symbol;
                var typeName = symbol.ToFullName();

                if (typeName.ToUnionType().IsNativeUnionType() == false
                    && symbol.IsValueType == true
                    && filtered.ContainsKey(typeName) == false
                )
                {
                    filtered[typeName] = candidate;
                }
            }

            using var typeRefs = ImmutableArrayBuilder<TypeRef>.Rent();
            typeRefs.AddRange(filtered.Values);

            TypeRefs = typeRefs.ToImmutable();
        }
    }

    public class TypeRef
    {
        public TypeSyntax Syntax { get; set; }

        public ITypeSymbol Symbol { get; set; }
    }
}
