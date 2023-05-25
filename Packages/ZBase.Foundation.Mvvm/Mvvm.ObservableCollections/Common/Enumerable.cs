using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public readonly struct Enumerable<T>
    {
        public readonly IEnumerable<T> Value;

        public Enumerable(IEnumerable<T> value)
        {
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RefTypeHandle(in Enumerable<T> value)
            => new(GCHandle.Alloc(value.Value, GCHandleType.Normal));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Enumerable<T>(in RefTypeHandle value)
            => new(value.Value.Target as IEnumerable<T>);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator RefTypeHandleUnion(in Enumerable<T> value)
            => new((RefTypeHandle)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Enumerable<T>(in RefTypeHandleUnion value)
            => value.Value;
    }
}