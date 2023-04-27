#pragma warning disable IDE0090 // Use 'new(...)'

using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ZBase.Foundation.Mvvm
{
    public readonly struct TypeId : IEquatable<TypeId>
    {
        public static readonly TypeId Null = default;

        private readonly uint _id;

        private TypeId(uint id)
        {
            _id = id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(TypeId other)
            => _id == other._id;


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is TypeId other && _id == other._id;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => _id.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => _id.ToString();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in TypeId lhs, in TypeId rhs)
            => lhs._id == rhs._id;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in TypeId lhs, in TypeId rhs)
            => lhs._id != rhs._id;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TypeId Of<T>()
            => new TypeId(Id<T>.Value);

        private static class Incrementer
        {
            private static readonly object s_lock = new();
            private static uint s_current;

            public static uint Next
            {
                get
                {
                    lock (s_lock)
                    {
                        Increment(ref s_current);
                        return s_current;
                    }
                }
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private static void Increment(ref uint location)
                => Interlocked.Add(ref Unsafe.As<uint, int>(ref location), 1);
        }

        private static class Id<T>
        {
            private static readonly uint s_value;

            public static uint Value => s_value;

            static Id()
            {
                s_value = Incrementer.Next;

#if UNITY_EDITOR
                UnityEngine.Debug.Log(
                    $"{nameof(TypeId)} {s_value} is assigned to {typeof(T)}.\n" +
                    $"If the value is overflowed, enabling Domain Reloading will reset it."
                );
#endif
            }
        }
    }
}
