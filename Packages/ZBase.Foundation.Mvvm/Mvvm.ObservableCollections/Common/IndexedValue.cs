using System.Runtime.CompilerServices;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public readonly struct IndexedValue
    {
        public readonly Union Value;
        public readonly int Index;

        public IndexedValue(int index, in Union value)
        {
            this.Index = index;
            this.Value = value;
        }
    }

    public readonly struct IndexedValue<T>
    {
        public readonly int Index;
        public readonly T Value;

        public IndexedValue(int index, T value)
        {
            this.Index = index;
            this.Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IndexedValue(in IndexedValue<T> value)
            => new(value.Index, value.Value.AsUnion());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IndexedValue<T>(in IndexedValue value)
            => new(value.Index, Union<T>.GetConverter().GetValue(value.Value));
    }

    public readonly partial struct IndexedValueUnion : IUnion<IndexedValue> { }
}
