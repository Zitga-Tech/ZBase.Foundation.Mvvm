using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public class ObservableHashSet<T> : IObservableCollection<T>, ISet<T>, IReadOnlyCollection<T>
    {
        private readonly object _syncRoot = new();
        private readonly HashSet<T> _collection;

        private event CollectionChangedEventHandler<T> _onChangingByAdd;
        private event CollectionChangedEventHandler<T> _onChangingByAddRange;
        private event CollectionChangedEventHandler<T> _onChangingByRemove;
        private event CollectionChangedEventHandler<T> _onChangingByRemoveRange;
        private event CollectionChangedEventHandler<T> _onChangingByClear;

        private event CollectionChangedEventHandler<T> _onChangedByAdd;
        private event CollectionChangedEventHandler<T> _onChangedByAddRange;
        private event CollectionChangedEventHandler<T> _onChangedByRemove;
        private event CollectionChangedEventHandler<T> _onChangedByRemoveRange;
        private event CollectionChangedEventHandler<T> _onChangedByClear;

        public ObservableHashSet()
        {
            _collection = new();
        }

        public ObservableHashSet(IEnumerable<T> collection)
        {
            _collection = new(collection);
        }

        public ObservableHashSet(IEqualityComparer<T> comparer)
        {
            _collection = new(comparer);
        }

        public ObservableHashSet(int capacity)
        {
            _collection = new(capacity);
        }

        public ObservableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            _collection = new(collection, comparer);
        }

        public ObservableHashSet(int capacity, IEqualityComparer<T> comparer)
        {
            _collection = new(capacity, comparer);
        }

        public object SyncRoot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _syncRoot;
        }

        public int Count
        {
            get
            {
                lock (_syncRoot)
                {
                    return _collection.Count;
                }
            }
        }

        public bool IsReadOnly => false;

        public bool Add(T item)
        {
            lock (_syncRoot)
            {
                _onChangingByAdd?.Invoke(CollectionChangeEventArgs<T>.Add(this, item));

                var result = _collection.Add(item);

                if (result)
                {
                    _onChangedByAdd?.Invoke(CollectionChangeEventArgs<T>.Add(this, item));
                }

                return result;
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            if (items == null)
            {
                throw new ArgumentNullException(nameof(items));
            }

            lock (SyncRoot)
            {
                _onChangingByAddRange?.Invoke(CollectionChangeEventArgs<T>.AddRange(this, items));

                if (items.TryGetCountFast(out var itemsCount) == false)
                {
                    itemsCount = 4;
                }

                var collection = _collection;
                var count = collection.Count;
                collection.EnsureCapacity(collection.Count + itemsCount);

                foreach (var item in items)
                {
                    collection.Add(item);
                }

                if (count != collection.Count)
                {
                    _onChangedByAddRange?.Invoke(CollectionChangeEventArgs<T>.AddRange(this, items));
                }
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _onChangingByClear?.Invoke(CollectionChangeEventArgs<T>.Clear(this));

                var collection = _collection;
                var count = collection.Count;
                collection.Clear();

                if (count != collection.Count)
                {
                    _onChangedByClear?.Invoke(CollectionChangeEventArgs<T>.Clear(this));
                }
            }
        }

        public bool Remove(T item)
        {
            lock (_syncRoot)
            {
                _onChangingByRemove?.Invoke(CollectionChangeEventArgs<T>.Remove(this, item));

                var result = _collection.Remove(item);

                if (result)
                {
                    _onChangedByRemove?.Invoke(CollectionChangeEventArgs<T>.Remove(this, item));
                }

                return result;
            }
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            lock (SyncRoot)
            {
                _onChangingByRemoveRange?.Invoke(CollectionChangeEventArgs<T>.RemoveRange(this, items));

                if (items.TryGetCountFast(out var itemsCount) == false)
                {
                    itemsCount = 4;
                }

                var collection = _collection;
                var count = collection.Count;

                foreach (var item in items)
                {
                    _collection.Remove(item);
                }

                if (count != collection.Count)
                {
                    _onChangedByRemoveRange?.Invoke(CollectionChangeEventArgs<T>.RemoveRange(this, items));
                }
            }
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            lock (_syncRoot)
            {
                return _collection.IsProperSubsetOf(other);
            }
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            lock (_syncRoot)
            {
                return _collection.IsProperSupersetOf(other);
            }
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            lock (_syncRoot)
            {
                return _collection.IsSubsetOf(other);
            }
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            lock (_syncRoot)
            {
                return _collection.IsSupersetOf(other);
            }
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            lock (_syncRoot)
            {
                return _collection.Overlaps(other);
            }
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            lock (_syncRoot)
            {
                return _collection.SetEquals(other);
            }
        }

        public bool Contains(T item)
        {
            lock (_syncRoot)
            {
                return _collection.Contains(item);
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_syncRoot)
            {
                _collection.CopyTo(array, arrayIndex);
            }
        }

        public HashSet<T>.Enumerator GetEnumerator()
        {
            lock (_syncRoot)
            {
                return _collection.GetEnumerator();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void ICollection<T>.Add(T item)
            => Add(item);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public bool CollectionChanging<TInstance>(CollectionAction action, CollectionChangeEventListener<T, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            switch (action)
            {
                case CollectionAction.Add:
                {
                    this._onChangingByAdd += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangingByAdd -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.AddRange:
                {
                    this._onChangingByAddRange += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangingByAddRange -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.Remove:
                {
                    this._onChangingByRemove += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangingByRemove -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.RemoveRange:
                {
                    this._onChangingByRemoveRange += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangingByRemoveRange -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.Clear:
                {
                    this._onChangingByClear += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangingByClear -= listener.OnEvent;
                    return true;
                }
            }

            return false;
        }

        public bool CollectionChanged<TInstance>(CollectionAction action, CollectionChangeEventListener<T, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            switch (action)
            {
                case CollectionAction.Add:
                {
                    this._onChangedByAdd += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangedByAdd -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.AddRange:
                {
                    this._onChangedByAddRange += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangedByAddRange -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.Remove:
                {
                    this._onChangedByRemove += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangedByRemove -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.RemoveRange:
                {
                    this._onChangedByRemoveRange += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangedByRemoveRange -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.Clear:
                {
                    this._onChangedByClear += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangedByClear -= listener.OnEvent;
                    return true;
                }
            }

            return false;
        }

        void ISet<T>.ExceptWith(IEnumerable<T> other) => throw new NotSupportedException();

        void ISet<T>.IntersectWith(IEnumerable<T> other) => throw new NotSupportedException();

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other) => throw new NotSupportedException();

        void ISet<T>.UnionWith(IEnumerable<T> other) => throw new NotSupportedException();
    }
}
