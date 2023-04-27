using System;

namespace ZBase.Foundation.Mvvm
{
    public static class TypeKindExtensions
    {
        public static TypeKind ToTypeKind(this TypeCode value)
            => value switch {
                TypeCode.Boolean => TypeKind.Bool,
                TypeCode.Byte => TypeKind.Byte,
                TypeCode.SByte => TypeKind.SByte,
                TypeCode.Char => TypeKind.Char,
                TypeCode.Decimal => TypeKind.Decimal,
                TypeCode.Double => TypeKind.Double,
                TypeCode.Single => TypeKind.Float,
                TypeCode.Int32 => TypeKind.Int,
                TypeCode.UInt32 => TypeKind.UInt,
                TypeCode.Int64 => TypeKind.Long,
                TypeCode.UInt64 => TypeKind.ULong,
                TypeCode.Int16 => TypeKind.Short,
                TypeCode.UInt16 => TypeKind.UShort,
                TypeCode.String => TypeKind.String,
                TypeCode.Object => TypeKind.Object,
                _ => TypeKind.Undefined,
            };

        public static EnumUnderlyingTypeKind ToEnumUnderlyingTypeKind(this TypeKind value)
            => value switch {
                TypeKind.Byte => EnumUnderlyingTypeKind.Byte,
                TypeKind.SByte => EnumUnderlyingTypeKind.SByte,
                TypeKind.Int => EnumUnderlyingTypeKind.Int,
                TypeKind.UInt => EnumUnderlyingTypeKind.UInt,
                TypeKind.Long => EnumUnderlyingTypeKind.Long,
                TypeKind.ULong => EnumUnderlyingTypeKind.ULong,
                TypeKind.Short => EnumUnderlyingTypeKind.Short,
                TypeKind.UShort => EnumUnderlyingTypeKind.UShort,
                _ => EnumUnderlyingTypeKind.Undefined,
            };

        public static EnumUnderlyingTypeKind ToEnumUnderlyingTypeKind(this TypeCode value)
            => value switch {
                TypeCode.Byte => EnumUnderlyingTypeKind.Byte,
                TypeCode.SByte => EnumUnderlyingTypeKind.SByte,
                TypeCode.Int32 => EnumUnderlyingTypeKind.Int,
                TypeCode.UInt32 => EnumUnderlyingTypeKind.UInt,
                TypeCode.Int64 => EnumUnderlyingTypeKind.Long,
                TypeCode.UInt64 => EnumUnderlyingTypeKind.ULong,
                TypeCode.Int16 => EnumUnderlyingTypeKind.Short,
                TypeCode.UInt16 => EnumUnderlyingTypeKind.UShort,
                _ => EnumUnderlyingTypeKind.Undefined,
            };

        public static EnumUnderlyingTypeKind ToEnumUnderlyingTypeKind<TEnum>(this TEnum value)
            where TEnum : unmanaged, System.Enum
            => value.GetTypeCode().ToEnumUnderlyingTypeKind();
    }
}
