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
    public class AsyncObservableDictionary<TKey, TValue> : IAsyncObservableCollection<KeyValuePair<TKey, TValue>>
    {
        private readonly object _syncRoot = new();
        private readonly Dictionary<TKey, TValue> _collection;

        private event AsyncCollectionChangedEventHandler<KeyValuePair<TKey, TValue>> _onChanging;
        private event AsyncCollectionChangedEventHandler<KeyValuePair<TKey, TValue>> _onChanged;

        public AsyncObservableDictionary()
        {
            _collection = new();
        }

        public AsyncObservableDictionary(IDictionary<TKey, TValue> dictionary)
        {
            _collection = new(dictionary);
        }

        public AsyncObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection)
        {
            _collection = new(collection);
        }

        public AsyncObservableDictionary(IEqualityComparer<TKey> comparer)
        {
            _collection = new(comparer);
        }

        public AsyncObservableDictionary(int capacity)
        {
            _collection = new(capacity);
        }

        public AsyncObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            _collection = new(dictionary, comparer);
        }

        public AsyncObservableDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer)
        {
            _collection = new(collection, comparer);
        }

        public AsyncObservableDictionary(int capacity, IEqualityComparer<TKey> comparer)
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

        public async Task Add(TKey key, TValue value)
        {
            var kv = new KeyValuePair<TKey, TValue>(key, value);

            if (_onChanging != null)
            {
                await _onChanging.Invoke(AsyncCollectionEventArgs.Add(this, kv));
            }

            var collection = _collection;
            var count = collection.Count;

            collection.Add(key, value);

            if (_onChanged != null && count != collection.Count)
            {
                await _onChanged.Invoke(AsyncCollectionEventArgs.Add(this, kv));
            }
        }

        public async Task Add(KeyValuePair<TKey, TValue> kv)
        {
            if (_onChanging != null)
            {
                await _onChanging.Invoke(AsyncCollectionEventArgs.Add(this, kv));
            }

            var collection = _collection;
            var count = collection.Count;

            collection.Add(kv.Key, kv.Value);

            if (_onChanged != null && count != collection.Count)
            {
                await _onChanged.Invoke(AsyncCollectionEventArgs.Add(this, kv));
            }
        }

        public async TaskBool Remove(TKey key)
        {
            var collection = _collection;

            if (collection.TryGetValue(key, out var value) == false)
            {
                return false;
            }

            var kv = new KeyValuePair<TKey, TValue>(key, value);

            if (_onChanging != null)
            {
                await _onChanging.Invoke(AsyncCollectionEventArgs.Replace(this, kv));
            }

            var result = _collection.Remove(key);

            if (result && _onChanged != null)
            {
                await _onChanged.Invoke(AsyncCollectionEventArgs.Remove(this, kv));
            }

            return result;
        }

        public async TaskBool Remove(KeyValuePair<TKey, TValue> item)
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

            if (_onChanging != null)
            {
                await _onChanging.Invoke(AsyncCollectionEventArgs.Remove(this, kv));
            }

            var result = collection.Remove(item.Key);

            if (result && _onChanged != null)
            {
                await _onChanged.Invoke(AsyncCollectionEventArgs.Remove(this, kv));
            }

            return result;
        }

        public async Task Set(TKey key, TValue value)
        {
            var kv = new KeyValuePair<TKey, TValue>(key, value);

            if (_onChanging != null)
            {
                await _onChanging.Invoke(AsyncCollectionEventArgs.Replace(this, kv));
            }

            _collection[key] = value;

            if (_onChanged != null)
            {
                await _onChanged.Invoke(AsyncCollectionEventArgs.Replace(this, kv));
            }
        }

        public async Task Clear()
        {
            if (_onChanging != null)
            {
                await _onChanging.Invoke(AsyncCollectionEventArgs.Clear(this));
            }

            var collection = _collection;
            var count = collection.Count;
            collection.Clear();

            if (_onChanged != null && count != collection.Count)
            {
                await _onChanged.Invoke(AsyncCollectionEventArgs.Clear(this));
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

        public void AttachCollectionChangingListener<TInstance>(AsyncCollectionEventListener<KeyValuePair<TKey, TValue>, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            this._onChanging += listener.OnEvent;
            listener.OnDetachAction = (listener) => this._onChanging -= listener.OnEvent;
        }

        public void AttachCollectionChangedListener<TInstance>(AsyncCollectionEventListener<KeyValuePair<TKey, TValue>, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            this._onChanged += listener.OnEvent;
            listener.OnDetachAction = (listener) => this._onChanged -= listener.OnEvent;
        }

        public async Task NotifyCollectionChanged<TInstance>(AsyncCollectionEventListener<KeyValuePair<TKey, TValue>, TInstance> listener)
            where TInstance : class
        {
            if (listener == null) throw new ArgumentNullException(nameof(listener));

            await listener.OnEvent(AsyncCollectionEventArgs.Undefined(this));
        }

        public async Task NotifyCollectionChanged()
        {
            if (this._onChanged == null) return;
            await this._onChanged.Invoke(AsyncCollectionEventArgs.Undefined(this));
        }
    }
}
