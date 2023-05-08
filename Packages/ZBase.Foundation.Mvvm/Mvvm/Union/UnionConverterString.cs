using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm.Unions.Converters
{
    internal sealed class UnionConverterString : IUnionConverter<string>
    {
        public static readonly UnionConverterString Default = new UnionConverterString();

        private UnionConverterString() { }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union ToUnion(string value) => new Union(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Union<string> ToUnionT(string value) => new Union(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryGetValue(in Union union, out string result) => union.TryGetValue(out result);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TrySetValueTo(in Union union, ref string dest) => union.TrySetValueTo(ref dest);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(in Union union) => union.GCHandle.Target?.ToString() ?? string.Empty;
    }
}
