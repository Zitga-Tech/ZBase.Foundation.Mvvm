using System;
using System.Diagnostics.CodeAnalysis;
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
        public T GetValue(in Union union)
        {
            ThrowIfInvalidCast();
            return default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(in Union union, out T result)
        {
            result = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TrySetValueTo(in Union union, ref T dest)
        {
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(in Union union)
        {
            return union.ToString();
        }

        [DoesNotReturn]
        private static void ThrowIfInvalidCast()
        {
            throw new InvalidCastException($"Cannot get value of {typeof(T)} from the input union.");
        }
    }
}
