using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public class ObservableQueue<T> : IObservableCollection<T>, IReadOnlyCollection<T>
    {
        private readonly object _syncRoot = new();
        private readonly Queue<T> _collection;

        private event CollectionChangedEventHandler<T> _onChanging;
        private event CollectionChangedEventHandler<T> _onChanged;

        public ObservableQueue()
        {
            _collection = new();
        }

        public ObservableQueue(IEnumerable<T> collection)
        {
            _collection = new(collection);
        }

        public ObservableQueue(int capacity)
        {
            _collection = new(capacity);
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

        public void Enqueue(T item)
        {
            lock (_syncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs<T>.Add(this, item));

                var collection = _collection;
                var count = collection.Count;
                _collection.Enqueue(item);

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs<T>.Add(this, item));
                }
            }
        }

        public T Dequeue()
        {
            lock (_syncRoot)
            {
                var collection = _collection;

                if (collection.TryPeek(out var item) == false)
                {
                    return default;
                }

                _onChanging?.Invoke(CollectionEventArgs<T>.Remove(this, item));

                var count = collection.Count;
                item = collection.Dequeue();

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs<T>.Remove(this, item));
                }

                return item;
            }
        }

        public bool TryDequeue(out T result)
        {
            lock (_syncRoot)
            {
                var collection = _collection;

                if (collection.TryPeek(out var item) == false)
                {
                    result = default;
                    return false;
                }

                _onChanging?.Invoke(CollectionEventArgs<T>.Remove(this, item));

                if (collection.TryDequeue(out item))
                {
                    result = item;
                    _onChanged?.Invoke(CollectionEventArgs<T>.Remove(this, item));

                    return true;
                }

                result = default;
                return false;
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

        public T Peek()
        {
            lock (_syncRoot)
            {
                return _collection.Peek();
            }
        }

        public bool TryPeek(out T result)
        {
            lock (_syncRoot)
            {
                return _collection.TryPeek(out result);
            }
        }

        public Queue<T>.Enumerator GetEnumerator()
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
    }
}
