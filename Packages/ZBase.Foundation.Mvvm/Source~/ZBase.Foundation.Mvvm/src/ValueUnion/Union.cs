using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZBase.Foundation.Mvvm
{
    /// <summary>
    /// A union of all supported values.
    /// </summary>
    /// <seealso cref="UnionBase" />
    [StructLayout(LayoutKind.Explicit)]
    public readonly partial struct Union
    {
        public static readonly Union Undefined = default;

        private static readonly string s_defaultStringForString = $"{nameof(Union)}.{TypeKind.String}";
        private static readonly string s_defaultStringForObject = $"{nameof(Union)}.{TypeKind.Object}";
        private static readonly string s_defaultStringForStruct = $"{nameof(Union)}.{TypeKind.Struct}";

        [FieldOffset(UnionBase.META_OFFSET)] public readonly UnionBase Base;
        [FieldOffset(UnionBase.META_OFFSET)] public readonly TypeKind Type;

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

        public Union(UnionBase @base, TypeKind type) : this()
        {
            Base = @base;
            Type = type;
        }

        public Union(TypeKind type) : this()
        {
            Type = type;
        }

        public Union(bool value) : this() { Type = TypeKind.Bool; Bool = value; }
        public Union(byte value) : this() { Type = TypeKind.Byte; Byte = value; }
        public Union(sbyte value) : this() { Type = TypeKind.SByte; SByte = value; }
        public Union(char value) : this() { Type = TypeKind.Char; Char = value; }
        public Union(double value) : this() { Type = TypeKind.Double; Double = value; }
        public Union(float value) : this() { Type = TypeKind.Float; Float = value; }
        public Union(int value) : this() { Type = TypeKind.Int; Int = value; }
        public Union(uint value) : this() { Type = TypeKind.UInt; UInt = value; }
        public Union(long value) : this() { Type = TypeKind.Long; Long = value; }
        public Union(ulong value) : this() { Type = TypeKind.ULong; ULong = value; }
        public Union(short value) : this() { Type = TypeKind.Short; Short = value; }
        public Union(ushort value) : this() { Type = TypeKind.UShort; UShort = value; }
        public Union(string value) : this() { Type = TypeKind.String; GCHandle = GCHandle.Alloc(value); }
        public Union(object value) : this() { Type = TypeKind.Object; GCHandle = GCHandle.Alloc(value); }

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
