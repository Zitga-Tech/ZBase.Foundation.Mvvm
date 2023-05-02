using System;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using ZBase.Foundation.Unions.Converters;

namespace ZBase.Foundation.Unions
{
    public static partial class UnionConverter
    {
        private static ConcurrentDictionary<UnionTypeId, object> s_converters;

        static UnionConverter()
        {
#if !UNITY_5_3_OR_NEWER || !UNITY_EDITOR
            Init();
#endif
        }

#if UNITY_5_3_OR_NEWER && UNITY_EDITOR
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
            if (converter == null)
                throw new ArgumentNullException(nameof(converter));

            return s_converters.TryAdd(UnionTypeId.Of<T>(), converter);
        }

        public static bool TryGetConverter<T>(out IUnionConverter<T> converter)
        {
            if (s_converters.TryGetValue(UnionTypeId.Of<T>(), out var candidate))
            {
                if (candidate is IUnionConverter<T> converterT)
                {
                    converter = converterT;
                    return true;
                }
            }

            if (UnionTypeId.TypeOf<T>().IsValueType == false)
            {
                converter = UnionConverterObject<T>.Default;
                return true;
            }

            converter = default;
            return false;
        }

        public static Union ToUnion<T>(T value)
        {
            if (TryGetConverter<T>(out var converter) == false)
            {
                ThrowConverterException<T>();
            }

            return converter.ToUnion(value);
        }

        public static Union<T> ToUnionT<T>(T value)
        {
            if (TryGetConverter<T>(out var converter) == false)
            {
                ThrowConverterException<T>();
            }

            return converter.ToUnionT(value);
        }

        public static bool TryGetValue<T>(in Union union, out T result)
        {
            if (TryGetConverter<T>(out var converter) == false)
            {
                ThrowConverterException<T>();
            }

            return converter.TryGetValue(union, out result);
        }

        public static bool TrySetValue<T>(in Union union, ref T dest)
        {
            if (TryGetConverter<T>(out var converter) == false)
            {
                ThrowConverterException<T>();
            }

            return converter.TrySetValue(union, ref dest);
        }

        [DoesNotReturn]
        private static void ThrowConverterException<T>()
        {
            throw new InvalidOperationException(
                $"Cannot find any {typeof(IUnionConverter<T>).Name}. " +
                $"Make sure to register the applicable converter before usage."
            );
        }

        [DoesNotReturn]
        private static void ThrowUndefinedException<T>()
        {
            throw new InvalidOperationException(
                $"Unexpected exception: Cannot get {nameof(UnionTypeKind)} value from {typeof(T)}."
            );
        }
    }
}
