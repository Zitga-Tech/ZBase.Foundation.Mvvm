using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.Unions.Converters
{
    internal sealed class UnionConverterUndefined<T> : IUnionConverter<T>
    {
        public static readonly UnionConverterUndefined<T> Default = new UnionConverterUndefined<T>();

        private UnionConverterUndefined() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union ToUnion(T value)
            => new Union(UnionTypeKind.Undefined, UnionTypeId.Of<T>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union<T> ToUnionT(T value)
            => new Union(UnionTypeKind.Undefined, UnionTypeId.Of<T>());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(in Union union, out T result)
        {
            result = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TrySetValue(in Union union, ref T dest)
        {
            return false;
        }
    }
}
