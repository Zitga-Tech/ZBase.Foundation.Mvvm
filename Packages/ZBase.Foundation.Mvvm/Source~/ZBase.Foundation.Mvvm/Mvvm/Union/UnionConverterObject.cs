﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.Unions.Converters
{
    internal sealed class UnionConverterObject : IUnionConverter<object>
    {
        public static readonly UnionConverterObject Default = new UnionConverterObject();

        private UnionConverterObject() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union ToUnion(object value)
            => new Union(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union<object> ToUnionT(object value)
            => new Union(value);

        public object GetValue(in Union union)
        {
            if (union.TryGetValue(out object result) == false)
            {
                ThrowIfInvalidCast();
            }

            return result;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(in Union union, out object result)
            => union.TryGetValue(out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TrySetValueTo(in Union union, ref object dest)
            => union.TrySetValueTo(ref dest);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(in Union union)
            => union.GCHandle.Target?.ToString() ?? string.Empty;

        [DoesNotReturn]
        private static void ThrowIfInvalidCast()
        {
            throw new InvalidCastException($"Cannot get value of {typeof(object)} from the input union.");
        }
    }
}
