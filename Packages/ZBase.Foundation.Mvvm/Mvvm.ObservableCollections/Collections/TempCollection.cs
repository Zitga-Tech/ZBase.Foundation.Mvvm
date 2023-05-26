using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    /// <summary>
    /// Read-only temporary collection.
    /// </summary>
    public struct TempCollection<T> : IReadOnlyCollection<T>, IDisposable
    {
        private static readonly bool s_canClearOnReturnToPool = RuntimeHelpers.IsReferenceOrContainsReferences<T>();
        private static readonly T[] s_emptyArray = Array.Empty<T>();

        private T[] _array;
        private int _count;

        public TempCollection(T item)
        {
            _array = ArrayPool<T>.Shared.Rent(1);
            _count = 1;
            _array[0] = item;
        }

        public TempCollection(ReadOnlySpan<T> source)
        {
            var array = ArrayPool<T>.Shared.Rent(source.Length);
            source.CopyTo(array);
            _array = array;
            _count = source.Length;
        }

        public TempCollection(IEnumerable<T> source)
        {
            if (source.TryGetCountFast(out var count))
            {
                var array = ArrayPool<T>.Shared.Rent(count);

                if (source is ICollection<T> c)
                {
                    c.CopyTo(array, 0);
                }
                else
                {
                    var i = 0;

                    foreach (var item in source)
                    {
                        array[i++] = item;
                    }
                }

                _array = array;
                _count = count;
            }
            else
            {
                var array = ArrayPool<T>.Shared.Rent(16);
                var i = 0;

                foreach (var item in source)
                {
                    TryEnsureCapacity(ref array, i);
                    array[i++] = item;
                }

                _array = array;
                _count = i;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlyMemory<T> AsMemory()
        {
            return _array.AsMemory(0, _count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlySpan<T> AsSpan()
        {
            return _array.AsSpan(0, _count);
        }

        public readonly int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _count;
        }

        public void Dispose()
        {
            if (_array != null && _array != s_emptyArray)
            {
                ArrayPool<T>.Shared.Return(_array, s_canClearOnReturnToPool);
                _array = s_emptyArray;
                _count = 0;
            }
        }

        private static void TryEnsureCapacity(ref T[] array, int index)
        {
            if (array.Length == index)
            {
                ArrayPool<T>.Shared.Return(array, s_canClearOnReturnToPool);
                array = ArrayPool<T>.Shared.Rent(index * 2);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Enumerator GetEnumerator()
            => new(_array, _count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        readonly IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly Enumerable AsEnumerable()
            => new(_array, _count);

        public class Enumerable : ICollection<T>
        {
            private readonly T[] _array;
            private readonly int _count;

            public Enumerable(T[] array, int count)
            {
                if (array == null)
                {
                    _array = Array.Empty<T>();
                    _count = 0;
                }
                else
                {
                    _array = array;
                    _count = count;
                }
            }

            public int Count
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _count;
            }

            public bool IsReadOnly => true;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void CopyTo(T[] dest, int destIndex)
                => Array.Copy(_array, 0, dest, destIndex, _count);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator GetEnumerator()
                => new(_array, _count);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            IEnumerator<T> IEnumerable<T>.GetEnumerator()
                => GetEnumerator();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            IEnumerator IEnumerable.GetEnumerator()
                => GetEnumerator();

            void ICollection<T>.Add(T item) => throw new NotSupportedException();

            void ICollection<T>.Clear() => throw new NotSupportedException();

            bool ICollection<T>.Contains(T item) => throw new NotSupportedException();

            bool ICollection<T>.Remove(T item) => throw new NotSupportedException();
        }

        public struct Enumerator : IEnumerator<T>
        {
            private readonly T[] _array;
            private readonly int _count;
            private int _index;

            public Enumerator(T[] array, int count)
            {
                _array = array;
                _count = count;
                _index = 0;
            }

            public readonly T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _array[_index];
            }

            public bool MoveNext()
            {
                if ((uint)_index < (uint)_count)
                {
                    _index++;
                    return true;
                }

                return false;
            }

            public void Reset()
            {
                _index = 0;
            }

            readonly object IEnumerator.Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => _array[_index];
            }

            public readonly void Dispose() { }
        }
    }
}