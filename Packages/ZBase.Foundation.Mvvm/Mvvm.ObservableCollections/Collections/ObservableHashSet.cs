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

        private event CollectionChangedEventHandler<T> _onChanging;
        private event CollectionChangedEventHandler<T> _onChanged;

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
                _onChanging?.Invoke(CollectionEventArgs<T>.Add(this, item));

                var result = _collection.Add(item);

                if (result)
                {
                    _onChanged?.Invoke(CollectionEventArgs<T>.Add(this, item));
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
                _onChanging?.Invoke(CollectionEventArgs<T>.AddRange(this, items));

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
                    _onChanged?.Invoke(CollectionEventArgs<T>.AddRange(this, items));
                }
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs<T>.Clear(this));

                var collection = _collection;
                var count = collection.Count;
                collection.Clear();

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs<T>.Clear(this));
                }
            }
        }

        public bool Remove(T item)
        {
            lock (_syncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs<T>.Remove(this, item));

                var result = _collection.Remove(item);

                if (result)
                {
                    _onChanged?.Invoke(CollectionEventArgs<T>.Remove(this, item));
                }

                return result;
            }
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            lock (SyncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs<T>.RemoveRange(this, items));

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
                    _onChanged?.Invoke(CollectionEventArgs<T>.RemoveRange(this, items));
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

        public void AttachCollectionChangingListener<TInstance>(CollectionEventListener<T, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            this._onChanging += listener.OnEvent;
            listener.OnDetachAction = (listener) => this._onChanging -= listener.OnEvent;
        }

        public void AttachCollectionChangedListener<TInstance>(CollectionEventListener<T, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            this._onChanged += listener.OnEvent;
            listener.OnDetachAction = (listener) => this._onChanged -= listener.OnEvent;
        }

        public void NotifyCollectionChanged<TInstance>(CollectionEventListener<T, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            listener.OnEvent(CollectionEventArgs<T>.Undefined(this));
        }

        public void NotifyCollectionChanged()
        {
            this._onChanged?.Invoke(CollectionEventArgs<T>.Undefined(this));
        }

        void ISet<T>.ExceptWith(IEnumerable<T> other) => throw new NotSupportedException();

        void ISet<T>.IntersectWith(IEnumerable<T> other) => throw new NotSupportedException();

        void ISet<T>.SymmetricExceptWith(IEnumerable<T> other) => throw new NotSupportedException();

        void ISet<T>.UnionWith(IEnumerable<T> other) => throw new NotSupportedException();
    }
}
