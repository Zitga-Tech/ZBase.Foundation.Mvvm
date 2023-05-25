using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public class ObservableList<T> : IObservableCollection<T>, IList<T>, IReadOnlyList<T>
    {
        private readonly object _syncRoot = new();
        private readonly List<T> _collection;

        private event CollectionChangedEventHandler<T> _onChangingByAdd;
        private event CollectionChangedEventHandler<T> _onChangingByInsert;
        private event CollectionChangedEventHandler<T> _onChangingByInsertRange;
        private event CollectionChangedEventHandler<T> _onChangingByAddRange;
        private event CollectionChangedEventHandler<T> _onChangingByRemoveAt;
        private event CollectionChangedEventHandler<T> _onChangingByRemoveRangeAt;
        private event CollectionChangedEventHandler<T> _onChangingByReplaceAt;
        private event CollectionChangedEventHandler<T> _onChangingByClear;

        private event CollectionChangedEventHandler<T> _onChangedByAdd;
        private event CollectionChangedEventHandler<T> _onChangedByInsert;
        private event CollectionChangedEventHandler<T> _onChangedByInsertRange;
        private event CollectionChangedEventHandler<T> _onChangedByAddRange;
        private event CollectionChangedEventHandler<T> _onChangedByRemoveAt;
        private event CollectionChangedEventHandler<T> _onChangedByRemoveRangeAt;
        private event CollectionChangedEventHandler<T> _onChangedByReplaceAt;
        private event CollectionChangedEventHandler<T> _onChangedByClear;
        
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
                    _onChangingByReplaceAt?.Invoke(CollectionChangeEventArgs<T>.ReplaceAt(this, index, value));

                    _collection[index] = value;

                    _onChangedByReplaceAt?.Invoke(CollectionChangeEventArgs<T>.ReplaceAt(this, index, value));
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
                _onChangingByAdd?.Invoke(CollectionChangeEventArgs<T>.Add(this, item));

                var collection = _collection;
                var count = collection.Count;
                _collection.Add(item);

                if (count != collection.Count)
                {
                    _onChangedByAdd?.Invoke(CollectionChangeEventArgs<T>.Add(this, item));
                }
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            lock (_syncRoot)
            {
                _onChangingByAddRange?.Invoke(CollectionChangeEventArgs<T>.AddRange(this, items));

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

        public void Insert(int index, T item)
        {
            lock (_syncRoot)
            {
                _onChangingByInsert?.Invoke(CollectionChangeEventArgs<T>.Insert(this, index, item));

                var collection = _collection;
                var count = collection.Count;
                collection.Insert(index, item);

                if (count != collection.Count)
                {
                    _onChangedByInsert?.Invoke(CollectionChangeEventArgs<T>.Insert(this, index, item));
                }
            }
        }
        
        public void InsertRange(int index, IEnumerable<T> items)
        {
            lock (_syncRoot)
            {
                _onChangingByInsertRange?.Invoke(CollectionChangeEventArgs<T>.InsertRange(this, index, items));

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
                    _onChangedByInsertRange?.Invoke(CollectionChangeEventArgs<T>.InsertRange(this, index, items));
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

                _onChangingByRemoveAt?.Invoke(CollectionChangeEventArgs<T>.RemoveAt(this, index));

                var count = collection.Count;
                collection.RemoveAt(index);

                var result = count != collection.Count;

                if (result)
                {
                    _onChangedByRemoveAt?.Invoke(CollectionChangeEventArgs<T>.RemoveAt(this, index));
                }

                return result;
            }
        }

        public void RemoveAt(int index)
        {
            lock (_syncRoot)
            {
                _onChangingByRemoveAt?.Invoke(CollectionChangeEventArgs<T>.RemoveAt(this, index));

                var collection = _collection;
                var count = collection.Count;
                collection.RemoveAt(index);

                if (count != collection.Count)
                {
                    _onChangedByRemoveAt?.Invoke(CollectionChangeEventArgs<T>.RemoveAt(this, index));
                }
            }
        }

        public void RemoveRangeAt(int index, int count)
        {
            lock (SyncRoot)
            {
                _onChangingByRemoveRangeAt?.Invoke(CollectionChangeEventArgs<T>.RemoveRangeAt(this, index, count));

                var collection = _collection;
                var collectionCount = collection.Count;
                _collection.RemoveRange(index, count);

                if (count != collection.Count)
                {
                    _onChangedByRemoveRangeAt?.Invoke(CollectionChangeEventArgs<T>.RemoveRangeAt(this, index, count));
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

                case CollectionAction.Insert:
                {
                    this._onChangingByInsert += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangingByInsert -= listener.OnEvent;
                    return true;
                }
                
                case CollectionAction.InsertRange:
                {
                    this._onChangingByInsertRange += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangingByInsertRange -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.AddRange:
                {
                    this._onChangingByAddRange += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangingByAddRange -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.RemoveAt:
                {
                    this._onChangingByRemoveAt += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangingByRemoveAt -= listener.OnEvent;
                    return true;
                }
                
                case CollectionAction.RemoveRangeAt:
                {
                    this._onChangingByRemoveRangeAt += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangingByRemoveRangeAt -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.ReplaceAt:
                {
                    this._onChangingByReplaceAt += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangingByReplaceAt -= listener.OnEvent;
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

                case CollectionAction.Insert:
                {
                    this._onChangedByInsert += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangedByInsert -= listener.OnEvent;
                    return true;
                }
                
                case CollectionAction.InsertRange:
                {
                    this._onChangedByInsertRange += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangedByInsertRange -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.AddRange:
                {
                    this._onChangedByAddRange += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangedByAddRange -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.RemoveAt:
                {
                    this._onChangedByRemoveAt += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangedByRemoveAt -= listener.OnEvent;
                    return true;
                }
                
                case CollectionAction.RemoveRangeAt:
                {
                    this._onChangedByRemoveRangeAt += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangedByRemoveRangeAt -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.ReplaceAt:
                {
                    this._onChangedByReplaceAt += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onChangedByReplaceAt -= listener.OnEvent;
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
    }
}
