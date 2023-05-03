using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ZBase.Foundation.Unions
{
    public readonly struct UnionTypeId : IEquatable<UnionTypeId>
    {
        public static readonly UnionTypeId Undefined = default;

        private readonly uint _id;

        private UnionTypeId(uint id)
        {
            _id = id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(UnionTypeId other)
            => _id == other._id;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object obj)
            => obj is UnionTypeId other && _id == other._id;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => _id.GetHashCode();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => _id.ToString();

        public Type AsType()
        {
            if (TypeVault.TryGetType(this, out var type))
                return type;

            return TypeVault.UndefinedType;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in UnionTypeId lhs, in UnionTypeId rhs)
            => lhs._id == rhs._id;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in UnionTypeId lhs, in UnionTypeId rhs)
            => lhs._id != rhs._id;

        public static UnionTypeId Of<T>()
        {
            var id = new UnionTypeId(Id<T>.Value);
            TypeVault.Register(id, Id<T>.Type);
            return id;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Type TypeOf<T>()
            => Id<T>.Type;

        private readonly struct UndefinedType { }

        private static class TypeVault
        {
            public static readonly Type UndefinedType = typeof(UndefinedType);

            private static ConcurrentDictionary<UnionTypeId, Type> s_vault = default;

            static TypeVault()
            {
                Init();
            }

#if UNITY_5_3_OR_NEWER
            [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
            private static void Init()
            {
                s_vault = new ConcurrentDictionary<UnionTypeId, Type>();
                s_vault.TryAdd(Undefined, UndefinedType);

                UnionTypeId.Of<bool>();
                UnionTypeId.Of<byte>();
                UnionTypeId.Of<sbyte>();
                UnionTypeId.Of<char>();
                UnionTypeId.Of<decimal>();
                UnionTypeId.Of<double>();
                UnionTypeId.Of<float>();
                UnionTypeId.Of<int>();
                UnionTypeId.Of<uint>();
                UnionTypeId.Of<long>();
                UnionTypeId.Of<ulong>();
                UnionTypeId.Of<short>();
                UnionTypeId.Of<ushort>();
                UnionTypeId.Of<string>();
                UnionTypeId.Of<object>();
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static void Register(UnionTypeId id, Type type)
                => s_vault.TryAdd(id, type);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static bool TryGetType(UnionTypeId id, out Type type)
                => s_vault.TryGetValue(id, out type);
        }

        private static class Incrementer
        {
            private static readonly object s_lock = new();
            private static uint s_current = default;

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

            public static readonly Type Type = typeof(T);

            static Id()
            {
                s_value = Incrementer.Next;

#if UNITY_5_3_OR_NEWER && UNITY_EDITOR && LOG_UNION_TYP_ID_REGISTRATION
                UnityEngine.Debug.Log(
                    $"{nameof(UnionTypeId)} {s_value} is assigned to {typeof(T)}. In case the value is overflowed, enabling Domain Reloading will reset it."
                );
#endif
            }
        }
    }
}
