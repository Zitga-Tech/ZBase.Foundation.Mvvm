#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
#pragma warning disable CA1815 // Override equals and operator equals on value types
#pragma warning disable CA1051 // Do not declare visible instance fields

using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.ObservableCollections.Async
{
    public readonly struct AsyncCollectionEventArgs<T>
    {
        public readonly CollectionAction Action;
        public readonly int Index;
        public readonly int Count;
        public readonly T Item;
        public readonly IEnumerable<T> Items;
        public readonly IAsyncObservableCollection<T> Sender;

        public AsyncCollectionEventArgs(
              IAsyncObservableCollection<T> sender
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
    }

    public static class AsyncCollectionEventArgs
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> Undefined<T>(IAsyncObservableCollection<T> sender)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.Undefined);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> Add<T>(IAsyncObservableCollection<T> sender, T item)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.Add, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> Insert<T>(IAsyncObservableCollection<T> sender, int index, T item)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.Insert, item, index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> InsertRange<T>(IAsyncObservableCollection<T> sender, int index, IEnumerable<T> items)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.InsertRange, index: index, items: items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> AddRange<T>(IAsyncObservableCollection<T> sender, IEnumerable<T> items)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.AddRange, items: items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> Remove<T>(IAsyncObservableCollection<T> sender, T item)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.Remove, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> RemoveRange<T>(IAsyncObservableCollection<T> sender, IEnumerable<T> items)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.RemoveRange, items: items);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> RemoveAt<T>(IAsyncObservableCollection<T> sender, int index)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.RemoveAt, index: index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> RemoveRangeAt<T>(IAsyncObservableCollection<T> sender, int index, int count)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.RemoveAt, index: index, count: count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> Replace<T>(IAsyncObservableCollection<T> sender, T item)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.Replace, item);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> ReplaceAt<T>(IAsyncObservableCollection<T> sender, int index, T item)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.ReplaceAt, item, index: index);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AsyncCollectionEventArgs<T> Clear<T>(IAsyncObservableCollection<T> sender)
        {
            return new AsyncCollectionEventArgs<T>(sender, CollectionAction.Clear);
        }
    }
}