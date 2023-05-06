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

        public ImmutableArray<TypeRef> ValueTypeRefs { get; }

        public ImmutableArray<TypeRef> RefTypeRefs { get; }

        public InternalUnionDeclaration(ImmutableArray<TypeRef> candidates)
        {
            var valueTypeFiltered = new Dictionary<string, TypeRef>();
            var refTypeFiltered = new Dictionary<string, TypeRef>();

            foreach (var candidate in candidates)
            {
                var symbol = candidate.Symbol;
                var typeName = symbol.ToFullName();

                if (typeName.ToUnionType().IsNativeUnionType())
                {
                    continue;
                }

                if (symbol.IsValueType)
                {
                    if (valueTypeFiltered.ContainsKey(typeName) == false)
                    {
                        valueTypeFiltered[typeName] = candidate;
                    }
                }
                else
                {
                    if (refTypeFiltered.ContainsKey(typeName) == false)
                    {
                        refTypeFiltered[typeName] = candidate;
                    }
                }
            }

            using var valueTypeRefs = ImmutableArrayBuilder<TypeRef>.Rent();
            using var refTypeRefs = ImmutableArrayBuilder<TypeRef>.Rent();

            valueTypeRefs.AddRange(valueTypeFiltered.Values);
            refTypeRefs.AddRange(refTypeFiltered.Values);

            ValueTypeRefs = valueTypeRefs.ToImmutable();
            RefTypeRefs = refTypeRefs.ToImmutable();
        }
    }

    public class TypeRef
    {
        public TypeSyntax Syntax { get; set; }

        public ITypeSymbol Symbol { get; set; }
    }
}
