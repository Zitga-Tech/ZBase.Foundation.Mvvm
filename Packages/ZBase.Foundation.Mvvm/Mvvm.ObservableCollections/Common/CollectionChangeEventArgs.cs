using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public readonly struct CollectionChangeEventArgs<T>
    {
        public readonly IObservableCollection<T> Sender;
        public readonly CollectionAction Action;
        public readonly Union Value;

        public CollectionChangeEventArgs(IObservableCollection<T> sender, CollectionAction action, in Union value)
        {
            Sender = sender;
            Action = action;
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> Add(IObservableCollection<T> sender, T item)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.Add, item.AsUnion());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> Insert(IObservableCollection<T> sender, int index, T item)
        {
            return new CollectionChangeEventArgs<T>(
                  sender
                , CollectionAction.Insert
                , new IndexedValueUnion(new IndexedValue<T>(index, item))
            );
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> InsertRange(IObservableCollection<T> sender, int index, IEnumerable<T> items)
        {
            return new CollectionChangeEventArgs<T>(
                  sender
                , CollectionAction.InsertRange
                , new IndexedEnumerableUnion(new IndexedEnumerable<T>(index, items))
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> AddRange(IObservableCollection<T> sender, IEnumerable<T> items)
        {
            return new CollectionChangeEventArgs<T>(
                  sender
                , CollectionAction.Insert
                , new RefTypeHandleUnion(new Enumerable<T>(items))
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> Remove(IObservableCollection<T> sender, T item)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.Remove, item.AsUnion());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> RemoveRange(IObservableCollection<T> sender, IEnumerable<T> items)
        {
            return new CollectionChangeEventArgs<T>(
                  sender
                , CollectionAction.RemoveRange
                , new RefTypeHandleUnion(new Enumerable<T>(items))
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> RemoveAt(IObservableCollection<T> sender, int index)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.RemoveAt, index);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> RemoveRangeAt(IObservableCollection<T> sender, int index, int count)
        {
            return new CollectionChangeEventArgs<T>(
                  sender
                , CollectionAction.RemoveRangeAt
                , new CollectionRange(index, count).AsUnion()
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> ReplaceAt(IObservableCollection<T> sender, int index, T item)
        {
            return new CollectionChangeEventArgs<T>(
                  sender
                , CollectionAction.ReplaceAt
                , new IndexedValueUnion(new IndexedValue<T>(index, item))
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> Clear(IObservableCollection<T> sender)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.Clear, Union.Undefined);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<KeyValuePair<TKey, TValue>> Add<TKey, TValue>(
              IObservableCollection<KeyValuePair<TKey, TValue>> sender
            , in KeyValuePair<TKey, TValue> item
        )
        {
            return new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>(
                  sender
                , CollectionAction.Add
                , new KeyedValueUnion(new KeyedValue<TKey, TValue>(item))
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<KeyValuePair<TKey, TValue>> Remove<TKey, TValue>(
              IObservableCollection<KeyValuePair<TKey, TValue>> sender
            , in KeyValuePair<TKey, TValue> item
        )
        {
            return new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>(
                  sender
                , CollectionAction.Remove
                , new KeyedValueUnion(new KeyedValue<TKey, TValue>(item))
            );
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<KeyValuePair<TKey, TValue>> Replace<TKey, TValue>(
              IObservableCollection<KeyValuePair<TKey, TValue>> sender
            , in KeyValuePair<TKey, TValue> item
        )
        {
            return new CollectionChangeEventArgs<KeyValuePair<TKey, TValue>>(
                  sender
                , CollectionAction.Replace
                , new KeyedValueUnion(new KeyedValue<TKey, TValue>(item))
            );
        }
    }
}