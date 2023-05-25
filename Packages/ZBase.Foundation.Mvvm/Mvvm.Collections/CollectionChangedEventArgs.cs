using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.Collections
{
    public readonly struct CollectionChangedEventArgs<T>
    {
        public readonly IObservableCollection<T> Sender;
        public readonly CollectionAction Action;
        public readonly Union Value;

        public CollectionChangedEventArgs(IObservableCollection<T> sender, CollectionAction action, in Union value)
        {
            Sender = sender;
            Action = action;
            Value = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangedEventArgs<T> Add(IObservableCollection<T> sender, T item)
        {
            return new CollectionChangedEventArgs<T>(sender, CollectionAction.Add, item.AsUnion());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangedEventArgs<T> Insert(IObservableCollection<T> sender, int index, T item)
        {
            var value = new IndexedItemUnion(new IndexedItem<T>(index, item));
            return new CollectionChangedEventArgs<T>(sender, CollectionAction.Insert, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangedEventArgs<T> AddRange(IObservableCollection<T> sender, IEnumerable<T> items)
        {
            var value = new RefTypeHandleUnion(new Enumerable<T>(items));
            return new CollectionChangedEventArgs<T>(sender, CollectionAction.Insert, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangedEventArgs<T> Remove(IObservableCollection<T> sender, T item)
        {
            return new CollectionChangedEventArgs<T>(sender, CollectionAction.Remove, item.AsUnion());
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangedEventArgs<T> RemoveAt(IObservableCollection<T> sender, int index)
        {
            return new CollectionChangedEventArgs<T>(sender, CollectionAction.RemoveAt, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangedEventArgs<T> Replace(IObservableCollection<T> sender, int index, T item)
        {
            var value = new IndexedItemUnion(new IndexedItem<T>(index, item));
            return new CollectionChangedEventArgs<T>(sender, CollectionAction.Replace, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangedEventArgs<T> Clear(IObservableCollection<T> sender)
        {
            return new CollectionChangedEventArgs<T>(sender, CollectionAction.Clear, Union.Undefined);
        }
    }
}