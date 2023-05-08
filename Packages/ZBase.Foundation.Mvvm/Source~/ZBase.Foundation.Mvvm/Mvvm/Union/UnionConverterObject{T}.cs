using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.Unions.Converters
{
    internal sealed class UnionConverterObject<T> : IUnionConverter<T>
    {
        public static readonly UnionConverterObject<T> Default = new UnionConverterObject<T>();

        private UnionConverterObject() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union ToUnion(T value)
            => new Union(UnionTypeId.Of<T>(), (object)value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union<T> ToUnionT(T value)
            => new Union(UnionTypeId.Of<T>(), (object)value);

        public bool TryGetValue(in Union union, out T result)
        {
            if (union.TryGetValue(out object candidate) && candidate is T value)
            {
                result = value;
                return true;
            }

            result = default;
            return false;
        }

        public bool TrySetValueTo(in Union union, ref T dest)
        {
            if (union.TryGetValue(out object candidate) && candidate is T value)
            {
                dest = value;
                return true;
            }

            return false;
        }

        public string ToString(in Union union)
        {
            if (union.TryGetValue(out object candidate) && candidate is T value)
            {
                return value.ToString();
            }

            return union.TypeId.AsType()?.ToString() ?? string.Empty;
        }
    }
}
