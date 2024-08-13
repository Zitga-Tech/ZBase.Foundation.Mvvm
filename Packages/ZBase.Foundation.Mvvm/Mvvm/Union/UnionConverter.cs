using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Unity.Collections.LowLevel.Unsafe;
using ZBase.Foundation.Mvvm.Unions.Converters;

#if UNITY_5_3_OR_NEWER
using UnityEngine;
#endif

namespace ZBase.Foundation.Mvvm.Unions
{
    public static partial class UnionConverter
    {
        private static ConcurrentDictionary<UnionTypeId, object> s_converters;

        static UnionConverter()
        {
            Init();
        }

#if UNITY_5_3_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        private static void Init()
        {
            s_converters = new ConcurrentDictionary<UnionTypeId, object>();

            TryRegister(UnionConverterBool.Default);
            TryRegister(UnionConverterByte.Default);
            TryRegister(UnionConverterSByte.Default);
            TryRegister(UnionConverterChar.Default);
            TryRegister(UnionConverterDouble.Default);
            TryRegister(UnionConverterFloat.Default);
            TryRegister(UnionConverterInt.Default);
            TryRegister(UnionConverterUInt.Default);
            TryRegister(UnionConverterLong.Default);
            TryRegister(UnionConverterULong.Default);
            TryRegister(UnionConverterShort.Default);
            TryRegister(UnionConverterUShort.Default);
            TryRegister(UnionConverterString.Default);
            TryRegister(UnionConverterObject.Default);
        }

        public static bool TryRegister<T>(IUnionConverter<T> converter)
        {
            ThrowIfNullOrSizeOfTIsBiggerThanUnionDataSize(converter);

            return s_converters.TryAdd(UnionTypeId.Of<T>(), converter);
        }

        public static IUnionConverter<T> GetConverter<T>()
        {
            if (s_converters.TryGetValue(UnionTypeId.Of<T>(), out var candidate))
            {
                if (candidate is IUnionConverter<T> converterT)
                {
                    return converterT;
                }
            }

            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                return UnionConverterObject<T>.Default;
            }

            return UnionConverterUndefined<T>.Default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Union ToUnion<T>(T value)
            => GetConverter<T>().ToUnion(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Union<T> ToUnionT<T>(T value)
            => GetConverter<T>().ToUnionT(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetValue<T>(in Union union)
            => GetConverter<T>().GetValue(union);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TryGetValue<T>(in Union union, out T result)
            => GetConverter<T>().TryGetValue(union, out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrySetValueTo<T>(in Union union, ref T dest)
            => GetConverter<T>().TrySetValueTo(union, ref dest);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ToString<T>(in Union union)
            => GetConverter<T>().ToString(union);

        [DoesNotReturn]
#if UNITY_5_3_OR_NEWER
        [HideInCallstack]
#endif
        private static void ThrowIfNullOrSizeOfTIsBiggerThanUnionDataSize<T>(IUnionConverter<T> converter)
        {
            if (converter == null)
            {
                throw new ArgumentNullException(nameof(converter));
            }

            var type = typeof(T);

            if (type.IsValueType == false)
            {
                return;
            }

            var sizeOfT = UnsafeUtility.SizeOf(type);

            if (sizeOfT > UnionData.SIZE)
            {
                throw new NotSupportedException(
                    $"The size of {typeof(T)} is {sizeOfT} bytes, " +
                    $"while a Union can only store {UnionData.SIZE} bytes of custom data. " +
                    $"To enable the automatic conversion between {typeof(T)} and {typeof(Union)}, please {GetDefineSymbolMessage(sizeOfT)}"
                );
            }

            static string GetDefineSymbolMessage(int size)
            {
                if (size > 128) return "contact the author to increase the maximum data size of the Union type.";
                if (size > 120) return "define UNION_SIZE_128_BYTES.";
                if (size > 112) return "define UNION_SIZE_120_BYTES.";
                if (size > 104) return "define UNION_SIZE_112_BYTES.";
                if (size > 96) return "define UNION_SIZE_104_BYTES.";
                if (size > 88) return "define UNION_SIZE_96_BYTES.";
                if (size > 80) return "define UNION_SIZE_88_BYTES.";
                if (size > 72) return "define UNION_SIZE_80_BYTES.";
                if (size > 64) return "define UNION_SIZE_72_BYTES.";
                if (size > 56) return "define UNION_SIZE_64_BYTES.";
                if (size > 48) return "define UNION_SIZE_56_BYTES.";
                if (size > 40) return "define UNION_SIZE_48_BYTES.";
                if (size > 32) return "define UNION_SIZE_40_BYTES.";
                if (size > 24) return "define UNION_SIZE_32_BYTES.";
                if (size > 16) return "define UNION_SIZE_24_BYTES.";
                if (size > 8) return "define UNION_SIZE_16_BYTES.";

                return "report to the author, because this is an unexpected error.";
            }
        }
    }
}
