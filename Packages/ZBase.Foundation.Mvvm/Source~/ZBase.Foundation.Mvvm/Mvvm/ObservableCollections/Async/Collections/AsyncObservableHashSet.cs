using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

#if ENABLE_UNITASK
using Task = Cysharp.Threading.Tasks.UniTask;
using TaskBool = Cysharp.Threading.Tasks.UniTask<bool>;
#else
using Task = System.Threading.Tasks.Task;
using TaskBool = System.Threading.Tasks.Task<bool>;
#endif

namespace ZBase.Foundation.Mvvm.ObservableCollections.Async
{
    public class AsyncObservableHashSet<T> : IAsyncObservableCollection<T>
    {
        private readonly object _syncRoot = new();
        private readonly HashSet<T> _collection;

        private event AsyncCollectionChangedEventHandler<T> _onChanging;
        private event AsyncCollectionChangedEventHandler<T> _onChanged;

        public AsyncObservableHashSet()
        {
            _collection = new();
        }

        public AsyncObservableHashSet(IEnumerable<T> collection)
        {
            _collection = new(collection);
        }

        public AsyncObservableHashSet(IEqualityComparer<T> comparer)
        {
            _collection = new(comparer);
        }

        public AsyncObservableHashSet(int capacity)
        {
            _collection = new(capacity);
        }

        public AsyncObservableHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
        {
            _collection = new(collection, comparer);
        }

        public AsyncObservableHashSet(int capacity, IEqualityComparer<T> comparer)
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
                _onChanging?.Invoke(AsyncCollectionEventArgs.Add(this, item));

                var result = _collection.Add(item);

                if (result)
                {
                    _onChanged?.Invoke(AsyncCollectionEventArgs.Add(this, item));
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
                _onChanging?.Invoke(AsyncCollectionEventArgs.AddRange(this, items));

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
                    _onChanged?.Invoke(AsyncCollectionEventArgs.AddRange(this, items));
                }
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _onChanging?.Invoke(AsyncCollectionEventArgs.Clear(this));

                var collection = _collection;
                var count = collection.Count;
                collection.Clear();

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(AsyncCollectionEventArgs.Clear(this));
                }
            }
        }

        public bool Remove(T item)
        {
            lock (_syncRoot)
            {
                _onChanging?.Invoke(AsyncCollectionEventArgs.Remove(this, item));

                var result = _collection.Remove(item);

                if (result)
                {
                    _onChanged?.Invoke(AsyncCollectionEventArgs.Remove(this, item));
                }

                return result;
            }
        }

        public void RemoveRange(IEnumerable<T> items)
        {
            lock (SyncRoot)
            {
                _onChanging?.Invoke(AsyncCollectionEventArgs.RemoveRange(this, items));

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
                    _onChanged?.Invoke(AsyncCollectionEventArgs.RemoveRange(this, items));
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
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public void AttachCollectionChangingListener<TInstance>(AsyncCollectionEventListener<T, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            this._onChanging += listener.OnEvent;
            listener.OnDetachAction = (listener) => this._onChanging -= listener.OnEvent;
        }

        public void AttachCollectionChangedListener<TInstance>(AsyncCollectionEventListener<T, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            this._onChanged += listener.OnEvent;
            listener.OnDetachAction = (listener) => this._onChanged -= listener.OnEvent;
        }

        public async Task NotifyCollectionChanged<TInstance>(AsyncCollectionEventListener<T, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            await listener.OnEvent(AsyncCollectionEventArgs.Undefined(this));
        }

        public async Task NotifyCollectionChanged()
        {
            if (this._onChanged != null)
            {
                await this._onChanged.Invoke(AsyncCollectionEventArgs.Undefined(this));
            }
        }
    }
}
