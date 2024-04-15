using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.ObservableCollections.Sync
{
    public class ObservableDictionary<TKey, TValue>
        : IObservableCollection<KeyValuePair<TKey, TValue>>
        , IDictionary<TKey, TValue>
        , IReadOnlyDictionary<TKey, TValue>
    {
        private readonly object _syncRoot = new();
        private readonly Dictionary<TKey, TValue> _collection;

        private event CollectionChangedEventHandler<KeyValuePair<TKey, TValue>> _onChanging;
        private event CollectionChangedEventHandler<KeyValuePair<TKey, TValue>> _onChanged;

        public ObservableDictionary()
        {
            _collection = new();
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _collection = new(dictionary);
        }

        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            _collection = new(collection);
        }

        public ObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            _collection = new(comparer);
        }

        public ObservableDictionary(int capacity)
        {
            _collection = new(capacity);
        }

        public ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _collection = new(dictionary, comparer);
        }

        public ObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
        {
            _collection = new(collection, comparer);
        }

        public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            _collection = new(capacity, comparer);
        }

        public Dictionary<TKey, TValue>.KeyCollection Keys
        {
            get
            {
                lock (_syncRoot)
                {
                    return _collection.Keys;
                }
            }
        }

        public Dictionary<TKey, TValue>.ValueCollection Values
        {
            get
            {
                lock (_syncRoot)
                {
                    return _collection.Values;
                }
            }
        }

        public TValue this[TKey key]
        {
            get
            {
                lock (_syncRoot)
                {
                    return _collection[key];
                }
            }

            set
            {
                lock (_syncRoot)
                {
                    var kv = new KeyValuePair<TKey, TValue>(key, value);
                    _onChanging?.Invoke(CollectionEventArgs.Replace(this, kv));

                    _collection[key] = value;

                    _onChanged?.Invoke(CollectionEventArgs.Replace(this, kv));
                }
            }
        }

        ICollection<TKey> IDictionary<TKey, TValue>.Keys
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Keys;
        }

        ICollection<TValue> IDictionary<TKey, TValue>.Values
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Values;
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Keys;
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Values;
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

        public void Add(TKey key, TValue value)
        {
            lock (SyncRoot)
            {
                var kv = new KeyValuePair<TKey, TValue>(key, value);
                _onChanging?.Invoke(CollectionEventArgs.Add(this, kv));

                var collection = _collection;
                var count = collection.Count;

                collection.Add(key, value);

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs.Add(this, kv));
                }
            }
        }

        public void Add(KeyValuePair<TKey, TValue> kv)
        {
            lock (SyncRoot)
            {
                _onChanging?.Invoke(CollectionEventArgs.Add(this, kv));

                var collection = _collection;
                var count = collection.Count;

                collection.Add(kv.Key, kv.Value);

                if (count != collection.Count)
                {
                    _onChanged?.Invoke(CollectionEventArgs.Add(this, kv));
                }
            }
        }

        public bool Remove(TKey key)
        {
            lock (_syncRoot)
            {
                var collection = _collection;

                if (collection.TryGetValue(key, out var value) == false)
                {
                    return false;
                }

                var kv = new KeyValuePair<TKey, TValue>(key, value);
                _onChanging?.Invoke(CollectionEventArgs.Replace(this, kv));

                var result = _collection.Remove(key);

                if (result)
                {
                    _onChanged?.Invoke(CollectionEventArgs.Remove(this, kv));
                }

                return result;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            lock (_syncRoot)
            {
                var collection = _collection;

                if (collection.TryGetValue(item.Key, out var value) == false)
                {
                    return false;
                }

                if (EqualityComparer<TValue>.Default.Equals(value, item.Value) == false)
                {
                    return false;
                }

                var kv = new KeyValuePair<TKey, TValue>(item.Key, value);
                _onChanging?.Invoke(CollectionEventArgs.Remove(this, kv));

                var result = collection.Remove(item.Key);

                if (result)
                {
                    _onChanged?.Invoke(CollectionEventArgs.Remove(this, kv));
                }

                return result;
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

        public bool ContainsKey(TKey key)
        {
            lock (_syncRoot)
            {
                return _collection.ContainsKey(key);
            }
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            lock (_syncRoot)
            {
                return ((ICollection<KeyValuePair<TKey, TValue>>)_collection).Contains(item);
            }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (_syncRoot)
            {
                return _collection.TryGetValue(key, out value);
            }
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            lock (_syncRoot)
            {
                ((ICollection<KeyValuePair<TKey, TValue>>)_collection).CopyTo(array, arrayIndex);
            }
        }

        public Dictionary<TKey, TValue>.Enumerator GetEnumerator()
        {
            lock (_syncRoot)
            {
                return _collection.GetEnumerator();
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
            => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public void AttachCollectionChangingListener<TInstance>(CollectionEventListener<KeyValuePair<TKey, TValue>, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            this._onChanging += listener.OnEvent;
            listener.OnDetachAction = (listener) => this._onChanging -= listener.OnEvent;
        }

        public void AttachCollectionChangedListener<TInstance>(CollectionEventListener<KeyValuePair<TKey, TValue>, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            this._onChanged += listener.OnEvent;
            listener.OnDetachAction = (listener) => this._onChanged -= listener.OnEvent;
        }

        public void NotifyCollectionChanged<TInstance>(CollectionEventListener<KeyValuePair<TKey, TValue>, TInstance> listener)
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
