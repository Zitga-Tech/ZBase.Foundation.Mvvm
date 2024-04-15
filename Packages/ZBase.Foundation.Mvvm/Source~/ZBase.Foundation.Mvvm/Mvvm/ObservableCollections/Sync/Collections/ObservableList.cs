using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.ObservableCollections.Sync
{
    public class ObservableList<T> : IObservableCollection<T>, IList<T>, IReadOnlyList<T>
    {
        private readonly object _syncRoot = new();
        private readonly List<T> _collection;

        private event CollectionChangedEventHandler<T> _onChanging;
        private event CollectionChangedEventHandler<T> _onChanged;
        
        public ObservableList()
        {
            _collection = new();
        }
        
        public ObservableList(IEnumerable<T> collection)
        {
            _collection = new(collection);
        }
        
        public ObservableList(int capacity)
        {
            _collection = new(capacity);
        }

        public object SyncRoot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _syncRoot;
        }

        public T this[int index]
        {
            get
            {
                lock (_syncRoot)
                {
                    return _collection[index];
                }
            }

            set
            {
                lock (_syncRoot)
                {
                    _onChanging?.Invoke(CollectionEventArgs.ReplaceAt(this, index, value));

                    _collection[index] = value;

                    _onChanged?.Invoke(CollectionEventArgs.ReplaceAt(this, index, value));
                }
            }
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

        public void Add(T item)
        {
            lock (_syncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs.Add(this, item));

                var collection = _collection;
                var count = collection.Count;
                _collection.Add(item);

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs.Add(this, item));
                }
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            lock (_syncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs.AddRange(this, items));

                if (items.TryGetCountFast(out var itemsCount) == false)
                {
                    itemsCount = 4;
                }

                var collection = _collection;
                var count = collection.Count;

                collection.Capacity = count + itemsCount;
                collection.AddRange(items);

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs.AddRange(this, items));
                }
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs.Clear(this));

                var collection = _collection;
                var count = collection.Count;
                collection.Clear();

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs.Clear(this));
                }
            }
        }

        public void Insert(int index, T item)
        {
            lock (_syncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs.Insert(this, index, item));

                var collection = _collection;
                var count = collection.Count;
                collection.Insert(index, item);

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs.Insert(this, index, item));
                }
            }
        }
        
        public void InsertRange(int index, IEnumerable<T> items)
        {
            lock (_syncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs.InsertRange(this, index, items));

                if (items.TryGetCountFast(out var itemsCount) == false)
                {
                    itemsCount = 4;
                }

                var collection = _collection;
                var count = collection.Count;

                collection.Capacity = count + itemsCount;
                collection.InsertRange(index, items);

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs.InsertRange(this, index, items));
                }
            }
        }

        public bool Remove(T item)
        {
            lock (_syncRoot)
            {
                var collection = _collection;
                var index = collection.IndexOf(item);

                if (index < 0)
                {
                    return false;
                }

                _onChanging?.Invoke(CollectionEventArgs.RemoveAt(this, index));

                var count = collection.Count;
                collection.RemoveAt(index);

                var result = count != collection.Count;

                if (result)
                {
                    _onChanged?.Invoke(CollectionEventArgs.RemoveAt(this, index));
                }

                return result;
            }
        }

        public void RemoveAt(int index)
        {
            lock (_syncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs.RemoveAt(this, index));

                var collection = _collection;
                var count = collection.Count;
                collection.RemoveAt(index);

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs.RemoveAt(this, index));
                }
            }
        }

        public void RemoveRangeAt(int index, int count)
        {
            lock (SyncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs.RemoveRangeAt(this, index, count));

                var collection = _collection;
                var collectionCount = collection.Count;
                _collection.RemoveRange(index, count);

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs.RemoveRangeAt(this, index, count));
                }
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

        public List<T>.Enumerator GetEnumerator()
        {
            lock (_syncRoot)
            {
                return _collection.GetEnumerator();
            }
        }

        public int IndexOf(T item)
        {
            lock (_syncRoot)
            {
                return _collection.IndexOf(item);
            }
        }

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

            listener.OnEvent(CollectionEventArgs.Undefined(this));
        }

        public void NotifyCollectionChanged()
        {
            this._onChanged?.Invoke(CollectionEventArgs.Undefined(this));
        }
    }
}
