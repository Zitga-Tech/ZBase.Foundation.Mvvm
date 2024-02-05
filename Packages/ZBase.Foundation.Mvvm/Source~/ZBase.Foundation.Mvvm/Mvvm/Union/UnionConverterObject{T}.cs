using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

namespace ZBase.Foundation.Mvvm.Unions.Converters
{
    internal sealed class UnionConverterObject<T> : IUnionConverter<T>
    {
        public static readonly UnionConverterObject<T> Default = new UnionConverterObject<T>();

        private UnionConverterObject() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union ToUnion(T value)
            => new Union(UnionTypeId.Of<T>(), (object)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union<T> ToUnionT(T value)
            => new Union(UnionTypeId.Of<T>(), (object)value);

        public T GetValue(in Union union)
        {
            if (union.TryGetValue(out object candidate)
                && candidate is T value
            )
            {
                return value;
            }

            ThrowIfInvalidCast();
            return default;
        }

        public bool TryGetValue(in Union union, out T result)
        {
            if (union.TryGetValue(out object candidate) && candidate is T value)
            {
                result = value;
                return true;
            }

            result = default;
            return false;
        }

        public bool TrySetValueTo(in Union union, ref T dest)
        {
            if (union.TryGetValue(out object candidate) && candidate is T value)
            {
                dest = value;
                return true;
            }

            return false;
        }

        public string ToString(in Union union)
        {
            if (union.TryGetValue(out object candidate) && candidate is T value)
            {
                return value.ToString();
            }

            return union.TypeId.AsType()?.ToString() ?? string.Empty;
        }

        [DoesNotReturn]
#if UNITY_5_3_OR_NEWER
        [HideInCallstack]
#endif
        private static void ThrowIfInvalidCast()
        {
            throw new InvalidCastException($"Cannot get value of {typeof(T)} from the input union.");
        }
    }
}
