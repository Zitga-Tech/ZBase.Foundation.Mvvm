using System.Runtime.CompilerServices;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.Collections
{
    public readonly struct IndexedItem
    {
        public readonly Union Item;
        public readonly int Index;

        public IndexedItem(int index, in Union item)
        {
            this.Index = index;
            this.Item = item;
        }
    }

    public readonly struct IndexedItem<T>
    {
        public readonly int Index;
        public readonly T Item;

        public IndexedItem(int index, T item)
        {
            this.Index = index;
            this.Item = item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IndexedItem(in IndexedItem<T> value)
            => new(value.Index, value.Item.AsUnion());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IndexedItem<T>(in IndexedItem value)
            => new(value.Index, Union<T>.GetConverter().GetValue(value.Item));
    }

    public readonly partial struct IndexedItemUnion : IUnion<IndexedItem> { }
}
