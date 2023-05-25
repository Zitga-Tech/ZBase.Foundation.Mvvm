using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public readonly struct CollectionRange
    {
        public readonly int Index;
        public readonly int Count;

        public CollectionRange(int index, int count)
        {
            Index = index;
            Count = count;
        }
    }

    public readonly partial struct CollectionRangeUnion : IUnion<CollectionRange> { }
}