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

        private static readonly string s_defaultStringForString = $"{nameof(ValueUnion)}.{TypeKind.String}";
        private static readonly string s_defaultStringForObject = $"{nameof(ValueUnion)}.{TypeKind.Object}";
        private static readonly string s_defaultStringForStruct = $"{nameof(ValueUnion)}.{TypeKind.Struct}";

        [FieldOffset(META_OFFSET)] public readonly ValueUnionStorage Storage;
        [FieldOffset(META_OFFSET)] public readonly uint Meta;

        [FieldOffset(TYPE_OFFSET)] public readonly TypeKind Type;

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

        public ValueUnion(ValueUnionStorage storage, TypeKind type) : this()
        {
            Storage = storage;
            Type = type;
        }

        public ValueUnion(TypeKind type) : this()
        {
            Type = type;
        }

        public ValueUnion(bool value)    : this() { Type = TypeKind.Bool; Bool = value; }
        public ValueUnion(byte value)    : this() { Type = TypeKind.Byte; Byte = value; }
        public ValueUnion(sbyte value)   : this() { Type = TypeKind.SByte; SByte = value; }
        public ValueUnion(char value)    : this() { Type = TypeKind.Char; Char = value; }
        public ValueUnion(double value)  : this() { Type = TypeKind.Double; Double = value; }
        public ValueUnion(float value)   : this() { Type = TypeKind.Float; Float = value; }
        public ValueUnion(int value)     : this() { Type = TypeKind.Int; Int = value; }
        public ValueUnion(uint value)    : this() { Type = TypeKind.UInt; UInt = value; }
        public ValueUnion(long value)    : this() { Type = TypeKind.Long; Long = value; }
        public ValueUnion(ulong value)   : this() { Type = TypeKind.ULong; ULong = value; }
        public ValueUnion(short value)   : this() { Type = TypeKind.Short; Short = value; }
        public ValueUnion(ushort value)  : this() { Type = TypeKind.UShort; UShort = value; }
        public ValueUnion(string value) : this() { Type = TypeKind.String; GCHandle = GCHandle.Alloc(value); }
        public ValueUnion(object value) : this() { Type = TypeKind.Object; GCHandle = GCHandle.Alloc(value); }

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
            => Type == other.Type;

        public bool TryGetValue(out bool dest)
        {
            if (Type == TypeKind.Bool)
            {
                dest = Bool; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out byte dest)
        {
            if (Type == TypeKind.Byte)
            {
                dest = Byte; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out sbyte dest)
        {
            if (Type == TypeKind.SByte)
            {
                dest = SByte; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out char dest)
        {
            switch (Type)
            {
                case TypeKind.SByte: dest = (char)SByte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.UShort: dest = (char)UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out double dest)
        {
            switch (Type)
            {
                case TypeKind.Byte  : dest = Byte ; return true;
                case TypeKind.SByte : dest = SByte; return true;
                case TypeKind.Char  : dest = Char; return true;
                case TypeKind.Double: dest = Double; return true;
                case TypeKind.Float : dest = Float; return true;
                case TypeKind.Int   : dest = Int; return true;
                case TypeKind.UInt  : dest = UInt; return true;
                case TypeKind.Long  : dest = Long; return true;
                case TypeKind.ULong : dest = ULong; return true;
                case TypeKind.Short : dest = Short; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out float dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.SByte: dest = SByte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.Float: dest = Float; return true;
                case TypeKind.Int: dest = Int; return true;
                case TypeKind.UInt: dest = UInt; return true;
                case TypeKind.Long: dest = Long; return true;
                case TypeKind.ULong: dest = ULong; return true;
                case TypeKind.Short: dest = Short; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out int dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.SByte: dest = SByte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.Int: dest = Int; return true;
                case TypeKind.Short: dest = Short; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out uint dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.UInt: dest = UInt; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out long dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.SByte: dest = SByte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.Int: dest = Int; return true;
                case TypeKind.UInt: dest = UInt; return true;
                case TypeKind.Long: dest = Long; return true;
                case TypeKind.Short: dest = Short; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out ulong dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.UInt: dest = UInt; return true;
                case TypeKind.ULong: dest = ULong; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out short dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.SByte: dest = SByte; return true;
                case TypeKind.Short: dest = Short; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out ushort dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out string dest)
        {
            if (Type == TypeKind.String && GCHandle.Target is string value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TryGetValue(out object dest)
        {
            if (Type == TypeKind.Object && GCHandle.Target is object value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TryGetValue<T>(out T dest)
        {
            if (Type == TypeKind.String && GCHandle.Target is T value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TrySetValue(ref bool dest)
        {
            if (Type == TypeKind.Bool)
            {
                dest = Bool; return true;
            }

            return false;
        }

        public bool TrySetValue(ref byte dest)
        {
            if (Type == TypeKind.Byte)
            {
                dest = Byte; return true;
            }

            return false;
        }

        public bool TrySetValue(ref sbyte dest)
        {
            if (Type == TypeKind.SByte)
            {
                dest = SByte; return true;
            }

            return false;
        }

        public bool TrySetValue(ref char dest)
        {
            switch (Type)
            {
                case TypeKind.SByte: dest = (char)SByte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.UShort: dest = (char)UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref double dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.SByte: dest = SByte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.Double: dest = Double; return true;
                case TypeKind.Float: dest = Float; return true;
                case TypeKind.Int: dest = Int; return true;
                case TypeKind.UInt: dest = UInt; return true;
                case TypeKind.Long: dest = Long; return true;
                case TypeKind.ULong: dest = ULong; return true;
                case TypeKind.Short: dest = Short; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref float dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.SByte: dest = SByte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.Float: dest = Float; return true;
                case TypeKind.Int: dest = Int; return true;
                case TypeKind.UInt: dest = UInt; return true;
                case TypeKind.Long: dest = Long; return true;
                case TypeKind.ULong: dest = ULong; return true;
                case TypeKind.Short: dest = Short; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref int dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.SByte: dest = SByte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.Int: dest = Int; return true;
                case TypeKind.Short: dest = Short; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref uint dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.UInt: dest = UInt; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref long dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.SByte: dest = SByte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.Int: dest = Int; return true;
                case TypeKind.UInt: dest = UInt; return true;
                case TypeKind.Long: dest = Long; return true;
                case TypeKind.Short: dest = Short; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref ulong dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.UInt: dest = UInt; return true;
                case TypeKind.ULong: dest = ULong; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref short dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.SByte: dest = SByte; return true;
                case TypeKind.Short: dest = Short; return true;
            }

            return false;
        }

        public bool TrySetValue(ref ushort dest)
        {
            switch (Type)
            {
                case TypeKind.Byte: dest = Byte; return true;
                case TypeKind.Char: dest = Char; return true;
                case TypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref string dest)
        {
            if (Type == TypeKind.String && GCHandle.Target is string value)
            {
                dest = value;
                return true;
            }

            return false;
        }

        public bool TrySetValue(ref object dest)
        {
            if (Type == TypeKind.Object && GCHandle.Target is object value)
            {
                dest = value;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            switch (Type)
            {
                case TypeKind.Bool: return Bool.ToString();
                case TypeKind.Byte: return Byte.ToString();
                case TypeKind.SByte: return SByte.ToString();
                case TypeKind.Char: return Char.ToString();
                case TypeKind.Double: return Double.ToString();
                case TypeKind.Float: return Float.ToString();
                case TypeKind.Int: return Int.ToString();
                case TypeKind.UInt: return UInt.ToString();
                case TypeKind.Long: return Long.ToString();
                case TypeKind.ULong: return ULong.ToString();
                case TypeKind.Short: return Short.ToString();
                case TypeKind.UShort: return UShort.ToString();

                case TypeKind.Struct: return s_defaultStringForStruct;

                case TypeKind.String:
                {
                    if (GCHandle.Target is string value)
                        return value;

                    return s_defaultStringForString;
                }

                case TypeKind.Object:
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
