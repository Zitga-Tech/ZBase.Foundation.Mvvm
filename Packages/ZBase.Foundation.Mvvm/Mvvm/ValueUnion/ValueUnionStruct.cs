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
    public readonly struct ValueUnionStruct<TStruct>
        where TStruct : struct
    {
        [FieldOffset(ValueUnion.META_OFFSET)] public readonly ValueUnion Base;

        [FieldOffset(ValueUnion.ENUM_TYPE_OFFSET + 1)] public readonly TypeId TypeId;
        [FieldOffset(ValueUnion.FIELD_OFFSET)] public readonly TStruct Struct;

        private ValueUnionStruct(in ValueUnion value) : this()
        {
            Base = value;
        }

        public ValueUnionStruct(TStruct value)
        {
            Base = new ValueUnion(TypeKind.Struct);
            TypeId = TypeId.Of<TStruct>();
            Struct = value;
        }

        public static implicit operator ValueUnion(in ValueUnionStruct<TStruct> value)
            => new ValueUnion(value.Base.Storage, TypeKind.Struct);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueUnionStruct<TStruct>(TStruct value)
            => new ValueUnionStruct<TStruct>(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator TStruct(in ValueUnionStruct<TStruct> value)
            => value.Struct;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueUnionStruct<TStruct>(in ValueUnion value)
            => new ValueUnionStruct<TStruct>(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TypeEquals(in ValueUnionStruct<TStruct> other)
            => Base.TypeEquals(other.Base);

        public bool TryGetValue(out TStruct dest)
        {
            if (Base.Type == TypeKind.Struct)
            {
                dest = Struct;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TrySetValue(ref TStruct dest)
        {
            if (Base.Type == TypeKind.Struct)
            {
                dest = Struct;
                return true;
            }

            return false;
        }
    }

    public static class ValueUnionStructExtensions
    {
        public static ValueUnion AsValueUnion<TStruct>(TStruct value)
            where TStruct : struct
            => new ValueUnionStruct<TStruct>(value);
    }
}
