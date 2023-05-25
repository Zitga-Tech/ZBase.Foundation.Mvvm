using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public readonly struct IndexedEnumerable
    {
        public readonly Union Items;
        public readonly int Index;

        public IndexedEnumerable(int index, in Union items)
        {
            this.Index = index;
            this.Items = items;
        }
    }

    public readonly struct IndexedEnumerable<T>
    {
        public readonly int Index;
        public readonly IEnumerable<T> Items;

        public IndexedEnumerable(int index, IEnumerable<T> items)
        {
            this.Index = index;
            this.Items = items;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator IndexedEnumerable(in IndexedEnumerable<T> value)
            => new(value.Index, (RefTypeHandleUnion)new Enumerable<T>(value.Items));

        public static implicit operator IndexedEnumerable<T>(in IndexedEnumerable value)
        {
            var enumerable = (Enumerable<T>)(new RefTypeHandleUnion(value.Items));
            return new(value.Index, enumerable.Value);
        }
    }

    public readonly partial struct IndexedEnumerableUnion : IUnion<IndexedEnumerable> { }
}
