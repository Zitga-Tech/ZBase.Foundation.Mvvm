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
    public readonly struct ValueUnionStruct<T>
        where T : struct
    {
        public static readonly TypeId StructTypeId = TypeId.Of<T>();

        [FieldOffset(ValueUnion.META_OFFSET)] public readonly ValueUnion Base;

        [FieldOffset(ValueUnion.TYPE_OFFSET + 1)] public readonly TypeId TypeId;
        [FieldOffset(ValueUnion.FIELD_OFFSET)] public readonly T Struct;

        private ValueUnionStruct(in ValueUnion value) : this()
        {
            Base = value;
        }

        public ValueUnionStruct(T value)
        {
            Base = new ValueUnion(ValueTypeCode.Struct);
            TypeId = StructTypeId;
            Struct = value;
        }

        public bool IsTypeValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Base.TypeCode == ValueTypeCode.Struct && TypeId == StructTypeId;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueUnion(in ValueUnionStruct<T> value)
            => new ValueUnion(value.Base.Storage, ValueTypeCode.Struct);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ValueUnionStruct<T>(T value)
            => new ValueUnionStruct<T>(value);

        public static explicit operator ValueUnionStruct<T>(in ValueUnion value)
        {
            var union = new ValueUnionStruct<T>(value);

            if (union.TypeId == StructTypeId)
                return union;

            throw new System.InvalidCastException($"Cannot cast value from {union.TypeId.AsType()} to {StructTypeId.AsType()}.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TypeEquals(in ValueUnionStruct<T> other)
            => Base.TypeEquals(other.Base);

        public bool TryGetValue(out T dest)
        {
            if (IsTypeValid)
            {
                dest = Struct;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TrySetValue(ref T dest)
        {
            if (IsTypeValid)
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
