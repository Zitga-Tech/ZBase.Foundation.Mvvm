#pragma warning disable IDE0090 // Use 'new(...)'

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZBase.Foundation.Mvvm
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ValueUnionStorage" />
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public readonly struct ValueUnionEnum<T>
        where T : unmanaged, System.Enum
    {
        public static readonly TypeKind UnderlyingType = default(T).ToEnumTypeKind();

        [FieldOffset(ValueUnion.META_OFFSET)] public readonly ValueUnion Base;
        [FieldOffset(ValueUnion.FIELD_OFFSET)] public readonly T Enum;

        private ValueUnionEnum(in ValueUnion value) : this()
        {
            Base = value;
        }

        public ValueUnionEnum(T value)
        {
            Base = new ValueUnion(UnderlyingType);
            Enum = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueUnion(in ValueUnionEnum<T> value)
            => new ValueUnion(value.Base.Storage, UnderlyingType);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueUnionEnum<T>(T value)
            => new ValueUnionEnum<T>(value);

        public static explicit operator ValueUnionEnum<T>(in ValueUnion value)
        {
            if (value.Type.IsEnumTypeKind())
                return new ValueUnionEnum<T>(value);

            throw new System.InvalidCastException($"Cannot cast value from {value.Type} into {nameof(UnderlyingType)}.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TypeEquals(in ValueUnionEnum<T> other)
            => Base.TypeEquals(other.Base);

        public bool TryGetValue(out T dest)
        {
            if (Base.Type.IsNumberImplicitlyConvertible(UnderlyingType))
            {
                dest = Enum;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TrySetValue(ref T dest)
        {
            if (Base.Type.IsNumberImplicitlyConvertible(UnderlyingType))
            {
                dest = Enum;
                return true;
            }

            return false;
        }
    }

    public static class ValueUnionEnumExtensions
    {
        public static ValueUnion AsValueUnion<TEnum>(TEnum value)
            where TEnum : unmanaged, System.Enum
            => new ValueUnionEnum<TEnum>(value);
    }
}
