#if MVVM_VALUE_UNION_SIZE_32 || MVVM_VALUE_UNION_SIZE_48 || MVVM_VALUE_UNION_SIZE_64 || MVVM_VALUE_UNION_SIZE_80 || MVVM_VALUE_UNION_SIZE_96 || MVVM_VALUE_UNION_SIZE_112 || MVVM_VALUE_UNION_SIZE_128
#define __MVVM_VALUE_UNION_SIZE_D2__
#endif

#if MVVM_VALUE_UNION_SIZE_48 || MVVM_VALUE_UNION_SIZE_64 || MVVM_VALUE_UNION_SIZE_80 || MVVM_VALUE_UNION_SIZE_96 || MVVM_VALUE_UNION_SIZE_112 || MVVM_VALUE_UNION_SIZE_128
#define __MVVM_VALUE_UNION_SIZE_D3__
#endif

#if MVVM_VALUE_UNION_SIZE_64 || MVVM_VALUE_UNION_SIZE_80 || MVVM_VALUE_UNION_SIZE_96 || MVVM_VALUE_UNION_SIZE_112 || MVVM_VALUE_UNION_SIZE_128
#define __MVVM_VALUE_UNION_SIZE_D4__
#endif

#if MVVM_VALUE_UNION_SIZE_80 || MVVM_VALUE_UNION_SIZE_96 || MVVM_VALUE_UNION_SIZE_112 || MVVM_VALUE_UNION_SIZE_128
#define __MVVM_VALUE_UNION_SIZE_D5__
#endif

#if MVVM_VALUE_UNION_SIZE_96 || MVVM_VALUE_UNION_SIZE_112 || MVVM_VALUE_UNION_SIZE_128
#define __MVVM_VALUE_UNION_SIZE_D6__
#endif

#if MVVM_VALUE_UNION_SIZE_112 || MVVM_VALUE_UNION_SIZE_128
#define __MVVM_VALUE_UNION_SIZE_D7__
#endif

#if MVVM_VALUE_UNION_SIZE_128
#define __MVVM_VALUE_UNION_SIZE_D8__
#endif

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZBase.Foundation.Mvvm
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct ValueUnion : IEquatable<ValueUnion>
    {
        public const int FIELD_OFFSET = 2;

        public static readonly ValueUnion Undefined = default;
        public static readonly ValueUnion UndefinedEnum = new ValueUnion(TypeKind.Enum, EnumUnderlyingTypeKind.Undefined);

        [FieldOffset(0)] private readonly Storage _storage;
        [FieldOffset(0)] public readonly TypeKind Type;
        [FieldOffset(1)] public readonly EnumUnderlyingTypeKind EnumType;

        [FieldOffset(FIELD_OFFSET)] public readonly bool Bool;
        [FieldOffset(FIELD_OFFSET)] public readonly byte Byte;
        [FieldOffset(FIELD_OFFSET)] public readonly sbyte SByte;
        [FieldOffset(FIELD_OFFSET)] public readonly char Char;
        [FieldOffset(FIELD_OFFSET)] public readonly decimal Decimal;
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
        
        public ValueUnion(bool value)    : this() { this.Type = TypeKind.Bool; this.Bool = value; }
        public ValueUnion(byte value)    : this() { this.Type = TypeKind.Byte; this.Byte = value; }
        public ValueUnion(sbyte value)   : this() { this.Type = TypeKind.SByte; this.SByte = value; }
        public ValueUnion(char value)    : this() { this.Type = TypeKind.Char; this.Char = value; }
        public ValueUnion(decimal value) : this() { this.Type = TypeKind.Decimal; this.Decimal = value; }
        public ValueUnion(double value)  : this() { this.Type = TypeKind.Double; this.Double = value; }
        public ValueUnion(float value)   : this() { this.Type = TypeKind.Float; this.Float = value; }
        public ValueUnion(int value)     : this() { this.Type = TypeKind.Int; this.Int = value; }
        public ValueUnion(uint value)    : this() { this.Type = TypeKind.UInt; this.UInt = value; }
        public ValueUnion(long value)    : this() { this.Type = TypeKind.Long; this.Long = value; }
        public ValueUnion(ulong value)   : this() { this.Type = TypeKind.ULong; this.ULong = value; }
        public ValueUnion(short value)   : this() { this.Type = TypeKind.Short; this.Short = value; }
        public ValueUnion(ushort value)  : this() { this.Type = TypeKind.UShort; this.UShort = value; }
        public ValueUnion(string value) : this() { this.Type = TypeKind.String; this.GCHandle = GCHandle.Alloc(value); }
        public ValueUnion(object value) : this() { this.Type = TypeKind.Object; this.GCHandle = GCHandle.Alloc(value); }

        internal ValueUnion(EnumUnderlyingTypeKind enumType, byte value)   : this() { this.Type = TypeKind.Enum; this.EnumType = enumType; this.Byte = value; }
        internal ValueUnion(EnumUnderlyingTypeKind enumType, sbyte value)  : this() { this.Type = TypeKind.Enum; this.EnumType = enumType; this.SByte = value; }
        internal ValueUnion(EnumUnderlyingTypeKind enumType, int value)    : this() { this.Type = TypeKind.Enum; this.EnumType = enumType; this.Int = value; }
        internal ValueUnion(EnumUnderlyingTypeKind enumType, uint value)   : this() { this.Type = TypeKind.Enum; this.EnumType = enumType; this.UInt = value; }
        internal ValueUnion(EnumUnderlyingTypeKind enumType, long value)   : this() { this.Type = TypeKind.Enum; this.EnumType = enumType; this.Long = value; }
        internal ValueUnion(EnumUnderlyingTypeKind enumType, ulong value)  : this() { this.Type = TypeKind.Enum; this.EnumType = enumType; this.ULong = value; }
        internal ValueUnion(EnumUnderlyingTypeKind enumType, short value)  : this() { this.Type = TypeKind.Enum; this.EnumType = enumType; this.Short = value; }
        internal ValueUnion(EnumUnderlyingTypeKind enumType, ushort value) : this() { this.Type = TypeKind.Enum; this.EnumType = enumType; this.UShort = value; }

        internal ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType) : this()
        {
            this.Type = type;
            this.EnumType = enumType;
        }

        internal ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, byte value)   : this() { this.Type = type; this.EnumType = enumType; this.Byte = value; }
        internal ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, sbyte value)  : this() { this.Type = type; this.EnumType = enumType; this.SByte = value; }
        internal ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, int value)    : this() { this.Type = type; this.EnumType = enumType; this.Int = value; }
        internal ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, uint value)   : this() { this.Type = type; this.EnumType = enumType; this.UInt = value; }
        internal ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, long value)   : this() { this.Type = type; this.EnumType = enumType; this.Long = value; }
        internal ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, ulong value)  : this() { this.Type = type; this.EnumType = enumType; this.ULong = value; }
        internal ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, short value)  : this() { this.Type = type; this.EnumType = enumType; this.Short = value; }
        internal ValueUnion(TypeKind type, EnumUnderlyingTypeKind enumType, ushort value) : this() { this.Type = type; this.EnumType = enumType; this.UShort = value; }

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator decimal(in ValueUnion value) => value.Decimal;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator double(in ValueUnion value)  => value.Double;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator float(in ValueUnion value)   => value.Float;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator int(in ValueUnion value)     => value.Int;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator uint(in ValueUnion value)    => value.UInt;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator long(in ValueUnion value)    => value.Long;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator ulong(in ValueUnion value)   => value.ULong;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator short(in ValueUnion value)   => value.Short;
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static explicit operator ushort(in ValueUnion value)  => value.UShort;

        public bool Equals(ValueUnion other)
        {
            if (this.Type != other.Type)
                return false;
            
            switch (other.Type)
            {
                case TypeKind.Undefined: return this.Type == TypeKind.Undefined;
                case TypeKind.Bool     : return this.Bool == other.Bool;
                case TypeKind.Byte     : return this.Byte == other.Byte;
                case TypeKind.SByte    : return this.SByte == other.SByte;
                case TypeKind.Char     : return this.Char == other.Char;
                case TypeKind.Decimal  : return this.Decimal == other.Decimal;
                case TypeKind.Double   : return this.Double == other.Double;
                case TypeKind.Float    : return this.Float == other.Float;
                case TypeKind.Int      : return this.Int == other.Int;
                case TypeKind.UInt     : return this.UInt == other.UInt;
                case TypeKind.Long     : return this.Long == other.Long;
                case TypeKind.ULong    : return this.ULong == other.ULong;
                case TypeKind.Short    : return this.Short == other.Short;
                case TypeKind.UShort   : return this.UShort == other.UShort;
                case TypeKind.Enum     : return EnumEquals(other);

                case TypeKind.String:
                case TypeKind.Object:
                {
                    return Equals(this.GCHandle.Target, other.GCHandle.Target);
                }
            }

            return false;
        }

        private bool EnumEquals(in ValueUnion other)
        {
            if (this.EnumType != other.EnumType)
                return false;

            return other.EnumType switch {
                EnumUnderlyingTypeKind.Undefined => this.EnumType == EnumUnderlyingTypeKind.Undefined,
                EnumUnderlyingTypeKind.Byte => this.Byte == other.Byte,
                EnumUnderlyingTypeKind.SByte => this.SByte == other.SByte,
                EnumUnderlyingTypeKind.Int => this.Int == other.Int,
                EnumUnderlyingTypeKind.UInt => this.UInt == other.UInt,
                EnumUnderlyingTypeKind.Long => this.Long == other.Long,
                EnumUnderlyingTypeKind.ULong => this.ULong == other.ULong,
                EnumUnderlyingTypeKind.Short => this.Short == other.Short,
                EnumUnderlyingTypeKind.UShort => this.UShort == other.UShort,
                _ => false,
            };
        }

        public bool TypeEquals(in ValueUnion other)
            => this.Type == TypeKind.Enum && other.Type == TypeKind.Enum
            ? this.EnumType == other.EnumType
            : this.Type == other.Type;

        public override bool Equals(object obj)
            => obj is ValueUnion other && Equals(other);

        public override int GetHashCode()
        {
            var hashcode = new HashCode();
            hashcode.Add(Type);
            hashcode.Add(EnumType);

            switch (Type)
            {
                case TypeKind.Bool   : hashcode.Add(Bool   ); break;
                case TypeKind.Byte   : hashcode.Add(Byte   ); break;
                case TypeKind.SByte  : hashcode.Add(SByte  ); break;
                case TypeKind.Char   : hashcode.Add(Char   ); break;
                case TypeKind.Decimal: hashcode.Add(Decimal); break;
                case TypeKind.Double : hashcode.Add(Double ); break;
                case TypeKind.Float  : hashcode.Add(Float  ); break;
                case TypeKind.Int    : hashcode.Add(Int    ); break;
                case TypeKind.UInt   : hashcode.Add(UInt   ); break;
                case TypeKind.Long   : hashcode.Add(Long   ); break;
                case TypeKind.ULong  : hashcode.Add(ULong  ); break;
                case TypeKind.Short  : hashcode.Add(Short  ); break;
                case TypeKind.UShort : hashcode.Add(UShort ); break;

                case TypeKind.String:
                case TypeKind.Object :
                {
                    try { hashcode.Add(GCHandle.Target); }
                    catch { hashcode.Add(0); }
                    break;
                }

                case TypeKind.Enum:
                {
                    switch (EnumType)
                    {
                        case EnumUnderlyingTypeKind.Byte  : hashcode.Add(Byte); break;
                        case EnumUnderlyingTypeKind.SByte : hashcode.Add(SByte); break;
                        case EnumUnderlyingTypeKind.Int   : hashcode.Add(Int); break;
                        case EnumUnderlyingTypeKind.UInt  : hashcode.Add(UInt); break;
                        case EnumUnderlyingTypeKind.Long  : hashcode.Add(Long); break;
                        case EnumUnderlyingTypeKind.ULong : hashcode.Add(ULong); break;
                        case EnumUnderlyingTypeKind.Short : hashcode.Add(Short); break;
                        case EnumUnderlyingTypeKind.UShort: hashcode.Add(UShort); break;
                    }

                    break;
                }
            }

            return hashcode.ToHashCode();
        }

        public override string ToString()
        {
            switch (Type)
            {
                case TypeKind.Bool   : return this.Bool.ToString();
                case TypeKind.Byte   : return this.Byte.ToString();
                case TypeKind.SByte  : return this.SByte.ToString();
                case TypeKind.Char   : return this.Char.ToString();
                case TypeKind.Decimal: return this.Decimal.ToString();
                case TypeKind.Double : return this.Double.ToString();
                case TypeKind.Float  : return this.Float.ToString();
                case TypeKind.Int    : return this.Int.ToString();
                case TypeKind.UInt   : return this.UInt.ToString();
                case TypeKind.Long   : return this.Long.ToString();
                case TypeKind.ULong  : return this.ULong.ToString();
                case TypeKind.Short  : return this.Short.ToString();
                case TypeKind.UShort : return this.UShort.ToString();

                case TypeKind.String :
                {
                    return GCHandle.Target is string value ? value : string.Empty;
                }

                case TypeKind.Object:
                {
                    return GCHandle.Target is object value ? value.ToString() : string.Empty;
                }

                case TypeKind.Enum:
                {
                    return EnumType switch {
                        EnumUnderlyingTypeKind.Byte => this.Byte.ToString(),
                        EnumUnderlyingTypeKind.SByte => this.SByte.ToString(),
                        EnumUnderlyingTypeKind.Int => this.Int.ToString(),
                        EnumUnderlyingTypeKind.UInt => this.UInt.ToString(),
                        EnumUnderlyingTypeKind.Long => this.Long.ToString(),
                        EnumUnderlyingTypeKind.ULong => this.ULong.ToString(),
                        EnumUnderlyingTypeKind.Short => this.Short.ToString(),
                        EnumUnderlyingTypeKind.UShort => this.UShort.ToString(),
                        _ => string.Empty,
                    };
                }
            }

            return string.Empty;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(in ValueUnion lhs, in ValueUnion rhs)
            => lhs.Equals(rhs);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(in ValueUnion lhs, in ValueUnion rhs)
            => !lhs.Equals(rhs);

        public bool TryGetValue(out bool dest)    { if (Type != TypeKind.Bool)    { dest = default; return false; } dest = this.Bool; return true; }
        public bool TryGetValue(out byte dest)    { if (Type != TypeKind.Byte)    { dest = default; return false; } dest = this.Byte; return true; }
        public bool TryGetValue(out sbyte dest)   { if (Type != TypeKind.SByte)   { dest = default; return false; } dest = this.SByte; return true; }
        public bool TryGetValue(out char dest)    { if (Type != TypeKind.Char)    { dest = default; return false; } dest = this.Char; return true; }
        public bool TryGetValue(out decimal dest) { if (Type != TypeKind.Decimal) { dest = default; return false; } dest = this.Decimal; return true; }
        public bool TryGetValue(out double dest)  { if (Type != TypeKind.Double)  { dest = default; return false; } dest = this.Double; return true; }
        public bool TryGetValue(out float dest)   { if (Type != TypeKind.Float)   { dest = default; return false; } dest = this.Float; return true; }
        public bool TryGetValue(out int dest)     { if (Type != TypeKind.Int)     { dest = default; return false; } dest = this.Int; return true; }
        public bool TryGetValue(out uint dest)    { if (Type != TypeKind.UInt)    { dest = default; return false; } dest = this.UInt; return true; }
        public bool TryGetValue(out long dest)    { if (Type != TypeKind.Long)    { dest = default; return false; } dest = this.Long; return true; }
        public bool TryGetValue(out ulong dest)   { if (Type != TypeKind.ULong)   { dest = default; return false; } dest = this.ULong; return true; }
        public bool TryGetValue(out short dest)   { if (Type != TypeKind.Short)   { dest = default; return false; } dest = this.Short; return true; }
        public bool TryGetValue(out ushort dest)  { if (Type != TypeKind.UShort)  { dest = default; return false; } dest = this.UShort; return true; }
        
        public bool TryGetValue(out string dest)
        {
            if (Type == TypeKind.String && this.GCHandle.Target is string value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TryGetValue(out object dest)
        {
            if (Type == TypeKind.Object && this.GCHandle.Target is object value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TryGetValue<T>(out T dest)
        {
            if (Type == TypeKind.String && this.GCHandle.Target is T value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TrySetValue(ref bool dest)    { if (Type != TypeKind.Bool)    { return false; } dest = this.Bool; return true; }
        public bool TrySetValue(ref byte dest)    { if (Type != TypeKind.Byte)    { return false; } dest = this.Byte; return true; }
        public bool TrySetValue(ref sbyte dest)   { if (Type != TypeKind.SByte)   { return false; } dest = this.SByte; return true; }
        public bool TrySetValue(ref char dest)    { if (Type != TypeKind.Char)    { return false; } dest = this.Char; return true; }
        public bool TrySetValue(ref decimal dest) { if (Type != TypeKind.Decimal) { return false; } dest = this.Decimal; return true; }
        public bool TrySetValue(ref double dest)  { if (Type != TypeKind.Double)  { return false; } dest = this.Double; return true; }
        public bool TrySetValue(ref float dest)   { if (Type != TypeKind.Float)   { return false; } dest = this.Float; return true; }
        public bool TrySetValue(ref int dest)     { if (Type != TypeKind.Int)     { return false; } dest = this.Int; return true; }
        public bool TrySetValue(ref uint dest)    { if (Type != TypeKind.UInt)    { return false; } dest = this.UInt; return true; }
        public bool TrySetValue(ref long dest)    { if (Type != TypeKind.Long)    { return false; } dest = this.Long; return true; }
        public bool TrySetValue(ref ulong dest)   { if (Type != TypeKind.ULong)   { return false; } dest = this.ULong; return true; }
        public bool TrySetValue(ref short dest)   { if (Type != TypeKind.Short)   { return false; } dest = this.Short; return true; }
        public bool TrySetValue(ref ushort dest)  { if (Type != TypeKind.UShort)  { return false; } dest = this.UShort; return true; }

        public bool TrySetValue(ref string dest)
        {
            if (Type == TypeKind.String && this.GCHandle.Target is string value)
            {
                dest = value;
                return true;
            }

            return false;
        }

        public bool TrySetValue(ref object dest)
        {
            if (Type == TypeKind.Object && this.GCHandle.Target is object value)
            {
                dest = value;
                return true;
            }

            return false;
        }

        public bool TrySetValue<T>(ref T dest)
        {
            if (Type == TypeKind.String && this.GCHandle.Target is T value)
            {
                dest = value;
                return true;
            }

            return false;
        }

        [StructLayout(LayoutKind.Sequential)]
        private readonly struct Storage
        {
            private readonly TypeKind _type;
            private readonly EnumUnderlyingTypeKind _enumType;
            private readonly decimal _d1;

#if __MVVM_VALUE_UNION_SIZE_D2__
        private readonly decimal _d2;
#endif

#if __MVVM_VALUE_UNION_SIZE_D3__
        private readonly decimal _d3;
#endif

#if __MVVM_VALUE_UNION_SIZE_D4__
        private readonly decimal _d4;
#endif

#if __MVVM_VALUE_UNION_SIZE_D5__
        private readonly decimal _d5;
#endif

#if __MVVM_VALUE_UNION_SIZE_D6__
        private readonly decimal _d6;
#endif

#if __MVVM_VALUE_UNION_SIZE_D7__
        private readonly decimal _d7;
#endif

#if __MVVM_VALUE_UNION_SIZE_D8__
        private readonly decimal _d8;
#endif
        }

    }
}
