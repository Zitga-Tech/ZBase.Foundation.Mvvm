using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZBase.Foundation.Mvvm.ComponentModel;

namespace ZBase.Foundation.Mvvm.Collections
{
    public class ObservableList<T> : IObservableCollection<T>, IList<T>, IReadOnlyList<T>
    {
        private readonly object _syncRoot = new();
        private readonly List<T> _collection;

        private event CollectionChangedEventHandler<T> _onAdd;
        private event CollectionChangedEventHandler<T> _onInsert;
        private event CollectionChangedEventHandler<T> _onAddRange;
        private event CollectionChangedEventHandler<T> _onRemove;
        private event CollectionChangedEventHandler<T> _onRemoveAt;
        private event CollectionChangedEventHandler<T> _onReplace;
        private event CollectionChangedEventHandler<T> _onClear;
        
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
                    var oldValue = _collection[index];
                    _collection[index] = value;
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
                _collection.Add(item);
                _onAdd?.Invoke(CollectionChangedEventArgs<T>.Add(this, item));
            }
        }

        public void AddRange(IEnumerable<T> items)
        {
            lock (_syncRoot)
            {
                _collection.AddRange(items);
                _onAdd?.Invoke(CollectionChangedEventArgs<T>.AddRange(this, items));
            }
        }

        public void Clear()
        {
            lock (_syncRoot)
            {
                _collection.Clear();
                _onClear?.Invoke(CollectionChangedEventArgs<T>.Clear(this));
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

        public void Insert(int index, T item)
        {
            lock (_syncRoot)
            {
                var collection = _collection;
                var count = collection.Count;
                collection.Insert(index, item);

                if (count != collection.Count)
                {
                    _onInsert?.Invoke(CollectionChangedEventArgs<T>.Insert(this, index, item));
                }
            }
        }

        public bool Remove(T item)
        {
            lock (_syncRoot)
            {
                var result = _collection.Remove(item);

                if (result)
                {
                    _onRemove?.Invoke(CollectionChangedEventArgs<T>.Remove(this, item));
                }

                return result;
            }
        }

        public void RemoveAt(int index)
        {
            lock (_syncRoot)
            {
                var collection = _collection;
                var count = collection.Count;
                collection.RemoveAt(index);

                if (count != collection.Count)
                {
                    _onRemoveAt?.Invoke(CollectionChangedEventArgs<T>.RemoveAt(this, index));
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public bool CollectionChanged<TInstance>(CollectionAction action, CollectionChangedEventListener<T, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            switch (action)
            {
                case CollectionAction.Add:
                {
                    this._onAdd += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onAdd -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.Insert:
                {
                    this._onInsert += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onInsert -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.AddRange:
                {
                    this._onAddRange += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onAddRange -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.Remove:
                {
                    this._onRemove += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onRemove -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.RemoveAt:
                {
                    this._onRemoveAt += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onRemoveAt -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.Replace:
                {
                    this._onReplace += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onReplace -= listener.OnEvent;
                    return true;
                }

                case CollectionAction.Clear:
                {
                    this._onClear += listener.OnEvent;
                    listener.OnDetachAction = (listener) => this._onClear -= listener.OnEvent;
                    return true;
                }
            }

            return false;
        }
    }
}
