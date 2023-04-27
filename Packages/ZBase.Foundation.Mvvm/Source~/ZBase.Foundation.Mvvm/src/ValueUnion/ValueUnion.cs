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
        public const int ENUM_TYPE_OFFSET = META_OFFSET + 1;

        public const int FIELD_OFFSET = META_OFFSET + META_SIZE;

        public static readonly ValueUnion Undefined = default;
        public static readonly ValueUnion UndefinedEnum = new ValueUnion(TypeKind.Enum, EnumUnderlyingTypeKind.Undefined);

        private static readonly string s_defaultStringForString = $"{nameof(ValueUnion)}.{TypeKind.String}";
        private static readonly string s_defaultStringForObject = $"{nameof(ValueUnion)}.{TypeKind.Object}";
        private static readonly string s_defaultStringForEnum = $"{nameof(ValueUnion)}.{TypeKind.Enum}";
        private static readonly string s_defaultStringForStruct = $"{nameof(ValueUnion)}.{TypeKind.Struct}";

        [FieldOffset(META_OFFSET)] public readonly ValueUnionStorage Storage;
        [FieldOffset(META_OFFSET)] public readonly uint Meta;

        [FieldOffset(TYPE_OFFSET)] public readonly TypeKind Type;
        [FieldOffset(ENUM_TYPE_OFFSET)] public readonly EnumUnderlyingTypeKind EnumType;

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

        public bool IsValid => IsEnum || Type != TypeKind.Undefined;

        public bool IsEnum => Type == TypeKind.Enum && EnumType != EnumUnderlyingTypeKind.Undefined;

        public ValueUnion(ValueUnionStorage storage) : this()
        {
            Storage = storage;
        }

        public ValueUnion(ValueUnionStorage storage, TypeKind type) : this()
        {
            Storage = storage;
            Type = type;
        }

        public ValueUnion(ValueUnionStorage storage, TypeKind type, EnumUnderlyingTypeKind enumType) : this()
        {
            Storage = storage;
            Type = type;
            EnumType = enumType;
        }

        public ValueUnion(TypeKind type) : this()
        {
            Type = type;
        }

        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType) : this()
        {
            Type = type;
            EnumType = enumType;
        }

        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, bool value)    : this() { Type = type; EnumType = enumType; Bool = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, byte value)    : this() { Type = type; EnumType = enumType; Byte = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, sbyte value)   : this() { Type = type; EnumType = enumType; SByte = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, char value)    : this() { Type = type; EnumType = enumType; Char = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, double value)  : this() { Type = type; EnumType = enumType; Double = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, float value)   : this() { Type = type; EnumType = enumType; Float = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, int value)     : this() { Type = type; EnumType = enumType; Int = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, uint value)    : this() { Type = type; EnumType = enumType; UInt = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, long value)    : this() { Type = type; EnumType = enumType; Long = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, ulong value)   : this() { Type = type; EnumType = enumType; ULong = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, short value)   : this() { Type = type; EnumType = enumType; Short = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, ushort value)  : this() { Type = type; EnumType = enumType; UShort = value; }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, string value)  : this() { Type = type; EnumType = enumType; GCHandle = GCHandle.Alloc(value); }
        public ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, object value)  : this() { Type = type; EnumType = enumType; GCHandle = GCHandle.Alloc(value); }
        
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator bool(in ValueUnion value)    => value.Bool;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator byte(in ValueUnion value)    => value.Byte;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator sbyte(in ValueUnion value)   => value.SByte;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator char(in ValueUnion value)    => value.Char;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator double(in ValueUnion value)  => value.Double;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator float(in ValueUnion value)   => value.Float;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator int(in ValueUnion value)     => value.Int;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator uint(in ValueUnion value)    => value.UInt;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator long(in ValueUnion value)    => value.Long;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator ulong(in ValueUnion value)   => value.ULong;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator short(in ValueUnion value)   => value.Short;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator ushort(in ValueUnion value)  => value.UShort;

        public bool TypeEquals(in ValueUnion other)
            => Type == TypeKind.Enum && other.Type == TypeKind.Enum
            ? EnumType == other.EnumType
            : Type == other.Type;

        public bool TryGetValue(out bool dest)    { if (Type != TypeKind.Bool)    { dest = default; return false; } dest = Bool; return true; }
        public bool TryGetValue(out byte dest)    { if (Type != TypeKind.Byte)    { dest = default; return false; } dest = Byte; return true; }
        public bool TryGetValue(out sbyte dest)   { if (Type != TypeKind.SByte)   { dest = default; return false; } dest = SByte; return true; }
        public bool TryGetValue(out char dest)    { if (Type != TypeKind.Char)    { dest = default; return false; } dest = Char; return true; }
        public bool TryGetValue(out double dest)  { if (Type != TypeKind.Double)  { dest = default; return false; } dest = Double; return true; }
        public bool TryGetValue(out float dest)   { if (Type != TypeKind.Float)   { dest = default; return false; } dest = Float; return true; }
        public bool TryGetValue(out int dest)     { if (Type != TypeKind.Int)     { dest = default; return false; } dest = Int; return true; }
        public bool TryGetValue(out uint dest)    { if (Type != TypeKind.UInt)    { dest = default; return false; } dest = UInt; return true; }
        public bool TryGetValue(out long dest)    { if (Type != TypeKind.Long)    { dest = default; return false; } dest = Long; return true; }
        public bool TryGetValue(out ulong dest)   { if (Type != TypeKind.ULong)   { dest = default; return false; } dest = ULong; return true; }
        public bool TryGetValue(out short dest)   { if (Type != TypeKind.Short)   { dest = default; return false; } dest = Short; return true; }
        public bool TryGetValue(out ushort dest)  { if (Type != TypeKind.UShort)  { dest = default; return false; } dest = UShort; return true; }
        
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

        public bool TrySetValue(ref bool dest)    { if (Type != TypeKind.Bool)    { return false; } dest = Bool; return true; }
        public bool TrySetValue(ref byte dest)    { if (Type != TypeKind.Byte)    { return false; } dest = Byte; return true; }
        public bool TrySetValue(ref sbyte dest)   { if (Type != TypeKind.SByte)   { return false; } dest = SByte; return true; }
        public bool TrySetValue(ref char dest)    { if (Type != TypeKind.Char)    { return false; } dest = Char; return true; }
        public bool TrySetValue(ref double dest)  { if (Type != TypeKind.Double)  { return false; } dest = Double; return true; }
        public bool TrySetValue(ref float dest)   { if (Type != TypeKind.Float)   { return false; } dest = Float; return true; }
        public bool TrySetValue(ref int dest)     { if (Type != TypeKind.Int)     { return false; } dest = Int; return true; }
        public bool TrySetValue(ref uint dest)    { if (Type != TypeKind.UInt)    { return false; } dest = UInt; return true; }
        public bool TrySetValue(ref long dest)    { if (Type != TypeKind.Long)    { return false; } dest = Long; return true; }
        public bool TrySetValue(ref ulong dest)   { if (Type != TypeKind.ULong)   { return false; } dest = ULong; return true; }
        public bool TrySetValue(ref short dest)   { if (Type != TypeKind.Short)   { return false; } dest = Short; return true; }
        public bool TrySetValue(ref ushort dest)  { if (Type != TypeKind.UShort)  { return false; } dest = UShort; return true; }

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

                case TypeKind.Enum:
                {
                    return EnumType switch {
                        EnumUnderlyingTypeKind.Byte => Byte.ToString(),
                        EnumUnderlyingTypeKind.SByte => SByte.ToString(),
                        EnumUnderlyingTypeKind.Int => Int.ToString(),
                        EnumUnderlyingTypeKind.UInt => UInt.ToString(),
                        EnumUnderlyingTypeKind.Long => Long.ToString(),
                        EnumUnderlyingTypeKind.ULong => ULong.ToString(),
                        EnumUnderlyingTypeKind.Short => Short.ToString(),
                        EnumUnderlyingTypeKind.UShort => UShort.ToString(),
                        _ => s_defaultStringForEnum,
                    };
                }

                case TypeKind.Struct: return s_defaultStringForStruct;
            }

            return string.Empty;
        }
    }
}
