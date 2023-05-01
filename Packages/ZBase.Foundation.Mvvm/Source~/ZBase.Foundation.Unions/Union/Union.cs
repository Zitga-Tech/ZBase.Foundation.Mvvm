using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZBase.Foundation.Unions
{
    /// <summary>
    /// The union data structure provides a layout and mechanism
    /// to store several types within the same memory position.
    /// <br/>
    /// By default, the capacity for storing the actual data is 8 bytes.
    /// That means it can store any data whose native size is lesser than or
    /// equal to 8 bytes. To increase this capacity, follow the instruction
    /// at <see cref="UnionData"/>.
    /// </summary>
    /// <remarks>
    /// 
    /// </remarks>
    /// <seealso cref="UnionBase" />
    [StructLayout(LayoutKind.Explicit)]
    public readonly partial struct Union
    {
        public const int UNION_TYPE_KIND_SIZE = sizeof(UnionTypeKind);
        public const int UNION_TYPE_ID_OFFSET = UnionBase.META_OFFSET + UNION_TYPE_KIND_SIZE;

        public static readonly Union Undefined = default;

        private static readonly string s_defaultStringForString = $"{nameof(Union)}.{UnionTypeKind.String}";
        private static readonly string s_defaultStringForObject = $"{nameof(Union)}.{UnionTypeKind.Object}";
        private static readonly string s_defaultStringForStruct = $"{nameof(Union)}.{UnionTypeKind.Struct}";

        [FieldOffset(UnionBase.META_OFFSET)] public readonly UnionBase Base;
        [FieldOffset(UnionBase.META_OFFSET)] public readonly UnionTypeKind TypeKind;
        [FieldOffset(UNION_TYPE_ID_OFFSET)]  public readonly UnionTypeId TypeId;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly bool Bool;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly byte Byte;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly sbyte SByte;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly char Char;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly double Double;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly float Float;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly int Int;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly uint UInt;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly long Long;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly ulong ULong;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly short Short;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly ushort UShort;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly GCHandle GCHandle;

        public Union(UnionBase @base) : this()
        {
            Base = @base;
        }

        public Union(UnionBase @base, UnionTypeKind type) : this()
        {
            Base = @base;
            TypeKind = type;
        }

        public Union(UnionTypeKind type) : this()
        {
            TypeKind = type;
        }

        public Union(bool value) : this() { TypeKind = UnionTypeKind.Bool; Bool = value; }
        public Union(byte value) : this() { TypeKind = UnionTypeKind.Byte; Byte = value; }
        public Union(sbyte value) : this() { TypeKind = UnionTypeKind.SByte; SByte = value; }
        public Union(char value) : this() { TypeKind = UnionTypeKind.Char; Char = value; }
        public Union(double value) : this() { TypeKind = UnionTypeKind.Double; Double = value; }
        public Union(float value) : this() { TypeKind = UnionTypeKind.Float; Float = value; }
        public Union(int value) : this() { TypeKind = UnionTypeKind.Int; Int = value; }
        public Union(uint value) : this() { TypeKind = UnionTypeKind.UInt; UInt = value; }
        public Union(long value) : this() { TypeKind = UnionTypeKind.Long; Long = value; }
        public Union(ulong value) : this() { TypeKind = UnionTypeKind.ULong; ULong = value; }
        public Union(short value) : this() { TypeKind = UnionTypeKind.Short; Short = value; }
        public Union(ushort value) : this() { TypeKind = UnionTypeKind.UShort; UShort = value; }
        public Union(string value) : this() { TypeKind = UnionTypeKind.String; GCHandle = GCHandle.Alloc(value); }
        public Union(object value) : this() { TypeKind = UnionTypeKind.Object; GCHandle = GCHandle.Alloc(value); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(bool value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(byte value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(sbyte value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(char value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(decimal value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(double value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(float value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(int value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(uint value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(long value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(ulong value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(short value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(ushort value) => new Union(value);
        [MethodImpl(MethodImplOptions.AggressiveInlining)] public static implicit operator Union(string value) => new Union(value);

        public bool TypeEquals(in Union other)
            => TypeKind == other.TypeKind;

        public bool TryGetValue(out bool dest)
        {
            if (TypeKind == UnionTypeKind.Bool)
            {
                dest = Bool; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out byte dest)
        {
            if (TypeKind == UnionTypeKind.Byte)
            {
                dest = Byte; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out sbyte dest)
        {
            if (TypeKind == UnionTypeKind.SByte)
            {
                dest = SByte; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out char dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.SByte: dest = (char)SByte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.UShort: dest = (char)UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out double dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.SByte: dest = SByte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.Double: dest = Double; return true;
                case UnionTypeKind.Float: dest = Float; return true;
                case UnionTypeKind.Int: dest = Int; return true;
                case UnionTypeKind.UInt: dest = UInt; return true;
                case UnionTypeKind.Long: dest = Long; return true;
                case UnionTypeKind.ULong: dest = ULong; return true;
                case UnionTypeKind.Short: dest = Short; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out float dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.SByte: dest = SByte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.Float: dest = Float; return true;
                case UnionTypeKind.Int: dest = Int; return true;
                case UnionTypeKind.UInt: dest = UInt; return true;
                case UnionTypeKind.Long: dest = Long; return true;
                case UnionTypeKind.ULong: dest = ULong; return true;
                case UnionTypeKind.Short: dest = Short; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out int dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.SByte: dest = SByte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.Int: dest = Int; return true;
                case UnionTypeKind.Short: dest = Short; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out uint dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.UInt: dest = UInt; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out long dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.SByte: dest = SByte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.Int: dest = Int; return true;
                case UnionTypeKind.UInt: dest = UInt; return true;
                case UnionTypeKind.Long: dest = Long; return true;
                case UnionTypeKind.Short: dest = Short; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out ulong dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.UInt: dest = UInt; return true;
                case UnionTypeKind.ULong: dest = ULong; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out short dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.SByte: dest = SByte; return true;
                case UnionTypeKind.Short: dest = Short; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out ushort dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            dest = default; return false;
        }

        public bool TryGetValue(out string dest)
        {
            if (TypeKind == UnionTypeKind.String && GCHandle.Target is string value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TryGetValue(out object dest)
        {
            if (TypeKind == UnionTypeKind.Object && GCHandle.Target is object value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TryGetValue<T>(out T dest)
        {
            if (TypeKind == UnionTypeKind.String && GCHandle.Target is T value)
            {
                dest = value;
                return true;
            }

            dest = default;
            return false;
        }

        public bool TrySetValue(ref bool dest)
        {
            if (TypeKind == UnionTypeKind.Bool)
            {
                dest = Bool; return true;
            }

            return false;
        }

        public bool TrySetValue(ref byte dest)
        {
            if (TypeKind == UnionTypeKind.Byte)
            {
                dest = Byte; return true;
            }

            return false;
        }

        public bool TrySetValue(ref sbyte dest)
        {
            if (TypeKind == UnionTypeKind.SByte)
            {
                dest = SByte; return true;
            }

            return false;
        }

        public bool TrySetValue(ref char dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.SByte: dest = (char)SByte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.UShort: dest = (char)UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref double dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.SByte: dest = SByte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.Double: dest = Double; return true;
                case UnionTypeKind.Float: dest = Float; return true;
                case UnionTypeKind.Int: dest = Int; return true;
                case UnionTypeKind.UInt: dest = UInt; return true;
                case UnionTypeKind.Long: dest = Long; return true;
                case UnionTypeKind.ULong: dest = ULong; return true;
                case UnionTypeKind.Short: dest = Short; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref float dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.SByte: dest = SByte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.Float: dest = Float; return true;
                case UnionTypeKind.Int: dest = Int; return true;
                case UnionTypeKind.UInt: dest = UInt; return true;
                case UnionTypeKind.Long: dest = Long; return true;
                case UnionTypeKind.ULong: dest = ULong; return true;
                case UnionTypeKind.Short: dest = Short; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref int dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.SByte: dest = SByte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.Int: dest = Int; return true;
                case UnionTypeKind.Short: dest = Short; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref uint dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.UInt: dest = UInt; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref long dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.SByte: dest = SByte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.Int: dest = Int; return true;
                case UnionTypeKind.UInt: dest = UInt; return true;
                case UnionTypeKind.Long: dest = Long; return true;
                case UnionTypeKind.Short: dest = Short; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref ulong dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.UInt: dest = UInt; return true;
                case UnionTypeKind.ULong: dest = ULong; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref short dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.SByte: dest = SByte; return true;
                case UnionTypeKind.Short: dest = Short; return true;
            }

            return false;
        }

        public bool TrySetValue(ref ushort dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValue(ref string dest)
        {
            if (TypeKind == UnionTypeKind.String && GCHandle.Target is string value)
            {
                dest = value;
                return true;
            }

            return false;
        }

        public bool TrySetValue(ref object dest)
        {
            if (TypeKind == UnionTypeKind.Object && GCHandle.Target is object value)
            {
                dest = value;
                return true;
            }

            return false;
        }

        public override string ToString()
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Bool: return Bool.ToString();
                case UnionTypeKind.Byte: return Byte.ToString();
                case UnionTypeKind.SByte: return SByte.ToString();
                case UnionTypeKind.Char: return Char.ToString();
                case UnionTypeKind.Double: return Double.ToString();
                case UnionTypeKind.Float: return Float.ToString();
                case UnionTypeKind.Int: return Int.ToString();
                case UnionTypeKind.UInt: return UInt.ToString();
                case UnionTypeKind.Long: return Long.ToString();
                case UnionTypeKind.ULong: return ULong.ToString();
                case UnionTypeKind.Short: return Short.ToString();
                case UnionTypeKind.UShort: return UShort.ToString();

                case UnionTypeKind.Struct: return s_defaultStringForStruct;

                case UnionTypeKind.String:
                {
                    if (GCHandle.Target is string value)
                        return value;

                    return s_defaultStringForString;
                }

                case UnionTypeKind.Object:
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
