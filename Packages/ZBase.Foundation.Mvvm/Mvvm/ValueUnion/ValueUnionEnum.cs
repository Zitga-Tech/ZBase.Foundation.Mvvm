using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZBase.Foundation.Mvvm
{
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public readonly struct ValueUnionEnum<TEnum> : IEquatable<ValueUnionEnum<TEnum>>
        where TEnum : unmanaged, System.Enum
    {
        public static readonly EnumUnderlyingTypeKind UnderlyingType = default(TEnum).ToEnumUnderlyingTypeKind();

        [FieldOffset(0)] public readonly ValueUnion Base;
        [FieldOffset(ValueUnion.FIELD_OFFSET)] public readonly TEnum Enum;

        private ValueUnionEnum(in ValueUnion value) : this()
        {
            Base = value;
        }

        public ValueUnionEnum(TEnum value)
        {
            var enumType = value.ToEnumUnderlyingTypeKind();
            Base = new ValueUnion(TypeKind.Enum, enumType);
            Enum = value;
        }

        public static implicit operator ValueUnion(in ValueUnionEnum<TEnum> value)
            => value.Base.EnumType switch {
                EnumUnderlyingTypeKind.Byte   => new ValueUnion(TypeKind.Enum, EnumUnderlyingTypeKind.Byte, value.Base.Byte),
                EnumUnderlyingTypeKind.SByte  => new ValueUnion(TypeKind.Enum, EnumUnderlyingTypeKind.SByte, value.Base.SByte),
                EnumUnderlyingTypeKind.Int    => new ValueUnion(TypeKind.Enum, EnumUnderlyingTypeKind.Int, value.Base.Int),
                EnumUnderlyingTypeKind.UInt   => new ValueUnion(TypeKind.Enum, EnumUnderlyingTypeKind.UInt, value.Base.UInt),
                EnumUnderlyingTypeKind.Long   => new ValueUnion(TypeKind.Enum, EnumUnderlyingTypeKind.Long, value.Base.Long),
                EnumUnderlyingTypeKind.ULong  => new ValueUnion(TypeKind.Enum, EnumUnderlyingTypeKind.ULong, value.Base.ULong),
                EnumUnderlyingTypeKind.Short  => new ValueUnion(TypeKind.Enum, EnumUnderlyingTypeKind.Short, value.Base.Short),
                EnumUnderlyingTypeKind.UShort => new ValueUnion(TypeKind.Enum, EnumUnderlyingTypeKind.UShort, value.Base.UShort),
                _ => ValueUnion.UndefinedEnum,
            };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueUnionEnum<TEnum>(TEnum value) => new ValueUnionEnum<TEnum>(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator TEnum(in ValueUnionEnum<TEnum> value) => value.Enum;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueUnionEnum<TEnum>(in ValueUnion value) => new ValueUnionEnum<TEnum>(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(ValueUnionEnum<TEnum> other)
            => this.Base.Equals(other.Base);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TypeEquals(in ValueUnionEnum<TEnum> other)
            => this.Base.TypeEquals(other.Base);

        public override bool Equals(object obj)
            => obj is ValueUnionEnum<TEnum> other
            && Equals(other);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => this.Base.GetHashCode();

        public bool TryGetValue(out TEnum dest)
        {
            if (this.Base.Type == TypeKind.Enum
                && this.Base.EnumType == UnderlyingType
            )
            {
                dest = this.Enum;
                return true;
            }

            dest = default;
            return false;
        }
    }
}
