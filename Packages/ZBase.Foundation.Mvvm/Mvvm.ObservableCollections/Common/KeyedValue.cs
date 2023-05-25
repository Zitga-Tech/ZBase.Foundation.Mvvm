using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public readonly struct KeyedValue
    {
        public readonly Union Key;
        public readonly Union Value;

        public KeyedValue(in Union key, in Union value)
        {
            this.Key = key;
            this.Value = value;
        }
    }

    public readonly struct KeyedValue<TKey, TValue>
    {
        public readonly TKey Key;
        public readonly TValue Value;

        public KeyedValue(TKey key, TValue value)
        {
            this.Key = key;
            this.Value = value;
        }

        public KeyedValue(in KeyValuePair<TKey, TValue> kv)
        {
            this.Key = kv.Key;
            this.Value = kv.Value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator KeyedValue(in KeyedValue<TKey, TValue> kv)
            => new(kv.Key.AsUnion(), kv.Value.AsUnion());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator KeyedValue<TKey, TValue>(in KeyedValue value)
            => new(Union<TKey>.GetConverter().GetValue(value.Key), Union<TValue>.GetConverter().GetValue(value.Value));
    }

    public readonly partial struct KeyedValueUnion : IUnion<KeyedValue> { }
}
