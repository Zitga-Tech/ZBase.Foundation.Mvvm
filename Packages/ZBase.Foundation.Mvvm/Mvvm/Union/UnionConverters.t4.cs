






///*****************************///
///                             ///
/// This file is auto-generated ///
/// DO NOT manually modify it!  ///
///                             ///
///*****************************///

namespace ZBase.Foundation.Mvvm.Unions.Converters
{

    internal sealed class UnionConverterBool : IUnionConverter<bool>
    {
        public static readonly UnionConverterBool Default = new UnionConverterBool();

        private UnionConverterBool() { }

        public Union ToUnion(bool value) => new Union(value);

        public Union<bool> ToUnionT(bool value) => new Union(value);

        public bool TryGetValue(in Union union, out bool result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref bool dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterByte : IUnionConverter<byte>
    {
        public static readonly UnionConverterByte Default = new UnionConverterByte();

        private UnionConverterByte() { }

        public Union ToUnion(byte value) => new Union(value);

        public Union<byte> ToUnionT(byte value) => new Union(value);

        public bool TryGetValue(in Union union, out byte result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref byte dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterSByte : IUnionConverter<sbyte>
    {
        public static readonly UnionConverterSByte Default = new UnionConverterSByte();

        private UnionConverterSByte() { }

        public Union ToUnion(sbyte value) => new Union(value);

        public Union<sbyte> ToUnionT(sbyte value) => new Union(value);

        public bool TryGetValue(in Union union, out sbyte result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref sbyte dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterChar : IUnionConverter<char>
    {
        public static readonly UnionConverterChar Default = new UnionConverterChar();

        private UnionConverterChar() { }

        public Union ToUnion(char value) => new Union(value);

        public Union<char> ToUnionT(char value) => new Union(value);

        public bool TryGetValue(in Union union, out char result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref char dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterDouble : IUnionConverter<double>
    {
        public static readonly UnionConverterDouble Default = new UnionConverterDouble();

        private UnionConverterDouble() { }

        public Union ToUnion(double value) => new Union(value);

        public Union<double> ToUnionT(double value) => new Union(value);

        public bool TryGetValue(in Union union, out double result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref double dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterFloat : IUnionConverter<float>
    {
        public static readonly UnionConverterFloat Default = new UnionConverterFloat();

        private UnionConverterFloat() { }

        public Union ToUnion(float value) => new Union(value);

        public Union<float> ToUnionT(float value) => new Union(value);

        public bool TryGetValue(in Union union, out float result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref float dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterInt : IUnionConverter<int>
    {
        public static readonly UnionConverterInt Default = new UnionConverterInt();

        private UnionConverterInt() { }

        public Union ToUnion(int value) => new Union(value);

        public Union<int> ToUnionT(int value) => new Union(value);

        public bool TryGetValue(in Union union, out int result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref int dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterUInt : IUnionConverter<uint>
    {
        public static readonly UnionConverterUInt Default = new UnionConverterUInt();

        private UnionConverterUInt() { }

        public Union ToUnion(uint value) => new Union(value);

        public Union<uint> ToUnionT(uint value) => new Union(value);

        public bool TryGetValue(in Union union, out uint result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref uint dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterLong : IUnionConverter<long>
    {
        public static readonly UnionConverterLong Default = new UnionConverterLong();

        private UnionConverterLong() { }

        public Union ToUnion(long value) => new Union(value);

        public Union<long> ToUnionT(long value) => new Union(value);

        public bool TryGetValue(in Union union, out long result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref long dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterULong : IUnionConverter<ulong>
    {
        public static readonly UnionConverterULong Default = new UnionConverterULong();

        private UnionConverterULong() { }

        public Union ToUnion(ulong value) => new Union(value);

        public Union<ulong> ToUnionT(ulong value) => new Union(value);

        public bool TryGetValue(in Union union, out ulong result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref ulong dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterShort : IUnionConverter<short>
    {
        public static readonly UnionConverterShort Default = new UnionConverterShort();

        private UnionConverterShort() { }

        public Union ToUnion(short value) => new Union(value);

        public Union<short> ToUnionT(short value) => new Union(value);

        public bool TryGetValue(in Union union, out short result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref short dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterUShort : IUnionConverter<ushort>
    {
        public static readonly UnionConverterUShort Default = new UnionConverterUShort();

        private UnionConverterUShort() { }

        public Union ToUnion(ushort value) => new Union(value);

        public Union<ushort> ToUnionT(ushort value) => new Union(value);

        public bool TryGetValue(in Union union, out ushort result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref ushort dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterString : IUnionConverter<string>
    {
        public static readonly UnionConverterString Default = new UnionConverterString();

        private UnionConverterString() { }

        public Union ToUnion(string value) => new Union(value);

        public Union<string> ToUnionT(string value) => new Union(value);

        public bool TryGetValue(in Union union, out string result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref string dest) => union.TrySetValue(ref dest);
    }

    internal sealed class UnionConverterObject : IUnionConverter<object>
    {
        public static readonly UnionConverterObject Default = new UnionConverterObject();

        private UnionConverterObject() { }

        public Union ToUnion(object value) => new Union(value);

        public Union<object> ToUnionT(object value) => new Union(value);

        public bool TryGetValue(in Union union, out object result) => union.TryGetValue(out result);

        public bool TrySetValue(in Union union, ref object dest) => union.TrySetValue(ref dest);
    }


}
