using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Unions
{
    public readonly struct Union<T> : IUnion<T>
    {
        public readonly Union Value;

        public Union(in Union union)
        {
            Value = new Union(union.Base, union.TypeKind, UnionTypeId.Of<T>());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Union<T>(in Union union)
            => new Union<T>(union);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Union(in Union<T> union)
            => union.Value;
    }
}
