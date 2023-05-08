using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.Unions.Converters
{
    internal sealed class UnionConverterObject : IUnionConverter<object>
    {
        public static readonly UnionConverterObject Default = new UnionConverterObject();

        private UnionConverterObject() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union ToUnion(object value) => new Union(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union<object> ToUnionT(object value) => new Union(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(in Union union, out object result) => union.TryGetValue(out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TrySetValueTo(in Union union, ref object dest) => union.TrySetValueTo(ref dest);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(in Union union) => union.GCHandle.Target?.ToString() ?? string.Empty;
    }
}
