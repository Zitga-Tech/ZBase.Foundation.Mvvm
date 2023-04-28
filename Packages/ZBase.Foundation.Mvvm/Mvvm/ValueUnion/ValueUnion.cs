#pragma warning disable IDE0090 // Use 'new(...)'

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZBase.Foundation.Mvvm
{
    /// <summary>
    /// A union of all supported values.
    /// </summary>
    /// <seealso cref="ValueUnionStorage" />
    [StructLayout(LayoutKind.Explicit)]
    public readonly partial struct ValueUnion
    {
        public const int META_SIZE = sizeof(ulong);
        public const int META_OFFSET = 0;

        public const int TYPE_OFFSET = META_OFFSET + 0;
        public const int FIELD_OFFSET = META_OFFSET + META_SIZE;

        public static readonly ValueUnion Undefined = default;

        private static readonly string s_defaultStringForString = $"{nameof(ValueUnion)}.{ValueTypeCode.String}";
        private static readonly string s_defaultStringForObject = $"{nameof(ValueUnion)}.{ValueTypeCode.Object}";
        private static readonly string s_defaultStringForStruct = $"{nameof(ValueUnion)}.{ValueTypeCode.Struct}";

        [FieldOffset(META_OFFSET)] public readonly ValueUnionStorage Storage;
        [FieldOffset(META_OFFSET)] public readonly uint Meta;

        [FieldOffset(TYPE_OFFSET)] public readonly ValueTypeCode TypeCode;

        [FieldOffset(FIELD_OFFSET)] public readonly bool Bool;
        [FieldOffset(FIELD_OFFSET)] public readonly byte Byte;
        [FieldOffset(FIELD_OFFSET)] public readonly sbyte SByte;
        [FieldOffset(FIELD_OFFSET)] public readonly char Char;
        [FieldOffset(FIELD_OFFSET)] public readonly double Double;
        [FieldOffset(FIELD_OFFSET)] public readonly float Float;
        [FieldOffset(FIELD_OFFSET)] public readonly int Int;
        [FieldOffset(FIELD_OFFSET)] public readonly uint UInt;
        [FieldOffset(FIELD_OFFSET)] public readonly long Long;
        [FieldOffset(FIELD_OFFSET)] public readonly ulong ULong;
        [FieldOffset(FIELD_OFFSET)] public readonly short Short;
        [FieldOffset(FIELD_OFFSET)] public readonly ushort UShort;
        [FieldOffset(FIELD_OFFSET)] public readonly GCHandle GCHandle;

        public ValueUnion(ValueUnionStorage storage) : this()
        {
            Storage = storage;
        }

        public ValueUnion(ValueUnionStorage storage, ValueTypeCode type) : this()
        {
            Storage = storage;
            TypeCode = type;
        }

        public ValueUnion(ValueTypeCode type) : this()
        {
            TypeCode = type;
        }

        public ValueUnion(bool value)    : this() { TypeCode = ValueTypeCode.Bool; Bool = value; }
        public ValueUnion(byte value)    : this() { TypeCode = ValueTypeCode.Byte; Byte = value; }
        public ValueUnion(sbyte value)   : this() { TypeCode = ValueTypeCode.SByte; SByte = value; }
        public ValueUnion(char value)    : this() { TypeCode = ValueTypeCode.Char; Char = value; }
        public ValueUnion(double value)  : this() { TypeCode = ValueTypeCode.Double; Double = value; }
        public ValueUnion(float value)   : this() { TypeCode = ValueTypeCode.Float; Float = value; }
        public ValueUnion(int value)     : this() { TypeCode = ValueTypeCode.Int; Int = value; }
        public ValueUnion(uint value)    : this() { TypeCode = ValueTypeCode.UInt; UInt = value; }
        public ValueUnion(long value)    : this() { TypeCode = ValueTypeCode.Long; Long = value; }
        public ValueUnion(ulong value)   : this() { TypeCode = ValueTypeCode.ULong; ULong = value; }
        public ValueUnion(short value)   : this() { TypeCode = ValueTypeCode.Short; Short = value; }
        public ValueUnion(ushort value)  : this() { TypeCode = ValueTypeCode.UShort; UShort = value; }
        public ValueUnion(string value) : this() { TypeCode = ValueTypeCode.String; GCHandle = GCHandle.Alloc(value); }
        public ValueUnion(object value) : this() { TypeCode = ValueTypeCode.Object; GCHandle = GCHandle.Alloc(value); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(bool value)    => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(byte value)    => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(sbyte value)   => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(char value)    => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(decimal value) => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(double value)  => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(float value)   => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(int value)     => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(uint value)    => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(long value)    => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(ulong value)   => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(short value)   => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(ushort value)  => new ValueUnion(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator ValueUnion(string value)  => new ValueUnion(value);

        public bool TypeEquals(in ValueUnion other)
            => TypeCode == other.TypeCode;

        public bool TryGetValue(out bool dest)
        {
            if (TypeCode == ValueTypeCode.Bool)
            {
                dest = Bool; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out byte dest)
        {
            if (TypeCode == ValueTypeCode.Byte)
            {
                dest = Byte; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out sbyte dest)
        {
            if (TypeCode == ValueTypeCode.SByte)
            {
                dest = SByte; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out char dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.SByte: dest = (char)SByte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.UShort: dest = (char)UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out double dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte  : dest = Byte ; return true;
                case ValueTypeCode.SByte : dest = SByte; return true;
                case ValueTypeCode.Char  : dest = Char; return true;
                case ValueTypeCode.Double: dest = Double; return true;
                case ValueTypeCode.Float : dest = Float; return true;
                case ValueTypeCode.Int   : dest = Int; return true;
                case ValueTypeCode.UInt  : dest = UInt; return true;
                case ValueTypeCode.Long  : dest = Long; return true;
                case ValueTypeCode.ULong : dest = ULong; return true;
                case ValueTypeCode.Short : dest = Short; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out float dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.SByte: dest = SByte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.Float: dest = Float; return true;
                case ValueTypeCode.Int: dest = Int; return true;
                case ValueTypeCode.UInt: dest = UInt; return true;
                case ValueTypeCode.Long: dest = Long; return true;
                case ValueTypeCode.ULong: dest = ULong; return true;
                case ValueTypeCode.Short: dest = Short; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out int dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.SByte: dest = SByte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.Int: dest = Int; return true;
                case ValueTypeCode.Short: dest = Short; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out uint dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.UInt: dest = UInt; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out long dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.SByte: dest = SByte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.Int: dest = Int; return true;
                case ValueTypeCode.UInt: dest = UInt; return true;
                case ValueTypeCode.Long: dest = Long; return true;
                case ValueTypeCode.Short: dest = Short; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out ulong dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.UInt: dest = UInt; return true;
                case ValueTypeCode.ULong: dest = ULong; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out short dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.SByte: dest = SByte; return true;
                case ValueTypeCode.Short: dest = Short; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out ushort dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out string dest)
        {
            if (TypeCode == ValueTypeCode.String && GCHandle.Target is string value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TryGetValue(out object dest)
        {
            if (TypeCode == ValueTypeCode.Object && GCHandle.Target is object value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TryGetValue<T>(out T dest)
        {
            if (TypeCode == ValueTypeCode.String && GCHandle.Target is T value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TrySetValue(ref bool dest)
        {
            if (TypeCode == ValueTypeCode.Bool)
            {
                dest = Bool; return true;
            }

            return false;
        }

        public bool TrySetValue(ref byte dest)
        {
            if (TypeCode == ValueTypeCode.Byte)
            {
                dest = Byte; return true;
            }

            return false;
        }

        public bool TrySetValue(ref sbyte dest)
        {
            if (TypeCode == ValueTypeCode.SByte)
            {
                dest = SByte; return true;
            }

            return false;
        }

        public bool TrySetValue(ref char dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.SByte: dest = (char)SByte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.UShort: dest = (char)UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref double dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.SByte: dest = SByte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.Double: dest = Double; return true;
                case ValueTypeCode.Float: dest = Float; return true;
                case ValueTypeCode.Int: dest = Int; return true;
                case ValueTypeCode.UInt: dest = UInt; return true;
                case ValueTypeCode.Long: dest = Long; return true;
                case ValueTypeCode.ULong: dest = ULong; return true;
                case ValueTypeCode.Short: dest = Short; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref float dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.SByte: dest = SByte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.Float: dest = Float; return true;
                case ValueTypeCode.Int: dest = Int; return true;
                case ValueTypeCode.UInt: dest = UInt; return true;
                case ValueTypeCode.Long: dest = Long; return true;
                case ValueTypeCode.ULong: dest = ULong; return true;
                case ValueTypeCode.Short: dest = Short; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref int dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.SByte: dest = SByte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.Int: dest = Int; return true;
                case ValueTypeCode.Short: dest = Short; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref uint dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.UInt: dest = UInt; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref long dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.SByte: dest = SByte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.Int: dest = Int; return true;
                case ValueTypeCode.UInt: dest = UInt; return true;
                case ValueTypeCode.Long: dest = Long; return true;
                case ValueTypeCode.Short: dest = Short; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref ulong dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.UInt: dest = UInt; return true;
                case ValueTypeCode.ULong: dest = ULong; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref short dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.SByte: dest = SByte; return true;
                case ValueTypeCode.Short: dest = Short; return true;
            }

            return false;
        }

        public bool TrySetValue(ref ushort dest)
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Byte: dest = Byte; return true;
                case ValueTypeCode.Char: dest = Char; return true;
                case ValueTypeCode.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref string dest)
        {
            if (TypeCode == ValueTypeCode.String && GCHandle.Target is string value)
            {
                dest = value;
                return true;
            }

            return false;
        }

        public bool TrySetValue(ref object dest)
        {
            if (TypeCode == ValueTypeCode.Object && GCHandle.Target is object value)
            {
                dest = value;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            switch (TypeCode)
            {
                case ValueTypeCode.Bool: return Bool.ToString();
                case ValueTypeCode.Byte: return Byte.ToString();
                case ValueTypeCode.SByte: return SByte.ToString();
                case ValueTypeCode.Char: return Char.ToString();
                case ValueTypeCode.Double: return Double.ToString();
                case ValueTypeCode.Float: return Float.ToString();
                case ValueTypeCode.Int: return Int.ToString();
                case ValueTypeCode.UInt: return UInt.ToString();
                case ValueTypeCode.Long: return Long.ToString();
                case ValueTypeCode.ULong: return ULong.ToString();
                case ValueTypeCode.Short: return Short.ToString();
                case ValueTypeCode.UShort: return UShort.ToString();

                case ValueTypeCode.Struct: return s_defaultStringForStruct;

                case ValueTypeCode.String:
                {
                    if (GCHandle.Target is string value)
                        return value;

                    return s_defaultStringForString;
                }

                case ValueTypeCode.Object:
                {
                    if (GCHandle.Target is object value)
                        return value.ToString();

                    return s_defaultStringForObject;
                }
            }

            return string.Empty;
        }
    }
}
