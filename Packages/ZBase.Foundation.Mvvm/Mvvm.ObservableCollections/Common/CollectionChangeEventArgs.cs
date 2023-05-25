using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public readonly struct CollectionChangeEventArgs<T>
    {
        public readonly CollectionAction Action;
        public readonly int Index;
        public readonly int Count;
        public readonly T Item;
        public readonly IEnumerable<T> Items;
        public readonly IObservableCollection<T> Sender;

        public CollectionChangeEventArgs(
              IObservableCollection<T> sender
            , CollectionAction action
            , T item = default
            , int index = 0
            , int count = 0
            , IEnumerable<T> items = null
        )
        {
            Action = action;
            Index = index;
            Count = count;
            Item = item;
            Items = items ?? Enumerable.Empty<T>();
            Sender = sender;
        }

        public bool TryGetFromAdd(out T item)
        {
            if (this.Action == CollectionAction.Add)
            {
                item = this.Item;
                return true;
            }

            item = default;
            return false;
        }

        public bool TryGetFromInsert(out int index, out T item)
        {
            if (this.Action == CollectionAction.Insert)
            {
                index = this.Index;
                item = this.Item;
                return true;
            }

            index = default;
            item = default;
            return false;
        }

        public bool TryGetFromInsertRange(out int index, out IEnumerable<T> items)
        {
            if (this.Action == CollectionAction.InsertRange)
            {
                index = this.Index;
                items = this.Items;
                return true;
            }

            index = default;
            items = default;
            return false;
        }

        public bool TryGetFromAddRange(out IEnumerable<T> items)
        {
            if (this.Action == CollectionAction.AddRange)
            {
                items = this.Items;
                return true;
            }

            items = default;
            return false;
        }

        public bool TryGetFromRemove(out T item)
        {
            if (this.Action == CollectionAction.Remove)
            {
                item = this.Item;
                return true;
            }

            item = default;
            return false;
        }

        public bool TryGetFromRemoveRange(out IEnumerable<T> items)
        {
            if (this.Action == CollectionAction.RemoveRange)
            {
                items = this.Items;
                return true;
            }

            items = default;
            return false;
        }

        public bool TryGetFromRemoveAt(out int index)
        {
            if (this.Action == CollectionAction.RemoveAt)
            {
                index = this.Index;
                return true;
            }

            index = default;
            return false;
        }

        public bool TryGetFromRemoveRangeAt(out int index, out int count)
        {
            if (this.Action == CollectionAction.RemoveRangeAt)
            {
                index = this.Index;
                count = this.Count;
                return true;
            }

            index = default;
            count = default;
            return false;
        }

        public bool TryGetFromReplace(out T item)
        {
            if (this.Action == CollectionAction.Replace)
            {
                item = this.Item;
                return true;
            }

            item = default;
            return false;
        }

        public bool TryGetFromReplaceAt(out int index, out T item)
        {
            if (this.Action == CollectionAction.ReplaceAt)
            {
                index = this.Index;
                item = this.Item;
                return true;
            }

            index = default;
            item = default;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> Add(IObservableCollection<T> sender, T item)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.Add, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> Insert(IObservableCollection<T> sender, int index, T item)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.Insert, item, index);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> InsertRange(IObservableCollection<T> sender, int index, IEnumerable<T> items)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.InsertRange, index: index, items: items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> AddRange(IObservableCollection<T> sender, IEnumerable<T> items)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.AddRange, items: items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> Remove(IObservableCollection<T> sender, T item)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.Remove, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> RemoveRange(IObservableCollection<T> sender, IEnumerable<T> items)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.RemoveRange, items: items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> RemoveAt(IObservableCollection<T> sender, int index)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.RemoveAt, index: index);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> RemoveRangeAt(IObservableCollection<T> sender, int index, int count)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.RemoveAt, index: index, count: count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> Replace(IObservableCollection<T> sender, T item)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.Replace, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> ReplaceAt(IObservableCollection<T> sender, int index, T item)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.ReplaceAt, item, index: index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static CollectionChangeEventArgs<T> Clear(IObservableCollection<T> sender)
        {
            return new CollectionChangeEventArgs<T>(sender, CollectionAction.Clear);
        }
    }
}