using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace ZBase.Foundation.Mvvm.Unions
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
    [StructLayout(LayoutKind.Explicit, Size = UnionData.SIZE)]
    public readonly partial struct Union : IDisposable
    {
        public const int UNION_TYPE_KIND_SIZE = sizeof(UnionTypeKind);
        public const int UNION_TYPE_ID_OFFSET = UnionBase.META_OFFSET + UNION_TYPE_KIND_SIZE;

        public static readonly Union Undefined = default;

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

        public Union(UnionBase @base, UnionTypeKind type, UnionTypeId typeId) : this()
        {
            Base = @base;
            TypeKind = type;
            TypeId = typeId;
        }

        public Union(UnionTypeKind type, UnionTypeId typeId) : this()
        {
            TypeKind = type;
            TypeId = typeId;
        }

        public Union(UnionBase @base) : this()
        {
            Base = @base;
        }

        public Union(bool value)   : this() { TypeKind = UnionTypeKind.Bool  ; TypeId = UnionTypeId.Of<bool>(); Bool = value; }
        public Union(byte value)   : this() { TypeKind = UnionTypeKind.Byte  ; TypeId = UnionTypeId.Of<byte>(); Byte = value; }
        public Union(sbyte value)  : this() { TypeKind = UnionTypeKind.SByte ; TypeId = UnionTypeId.Of<sbyte>(); SByte = value; }
        public Union(char value)   : this() { TypeKind = UnionTypeKind.Char  ; TypeId = UnionTypeId.Of<char>(); Char = value; }
        public Union(double value) : this() { TypeKind = UnionTypeKind.Double; TypeId = UnionTypeId.Of<double>(); Double = value; }
        public Union(float value)  : this() { TypeKind = UnionTypeKind.Float ; TypeId = UnionTypeId.Of<float>(); Float = value; }
        public Union(int value)    : this() { TypeKind = UnionTypeKind.Int   ; TypeId = UnionTypeId.Of<int>(); Int = value; }
        public Union(uint value)   : this() { TypeKind = UnionTypeKind.UInt  ; TypeId = UnionTypeId.Of<uint>(); UInt = value; }
        public Union(long value)   : this() { TypeKind = UnionTypeKind.Long  ; TypeId = UnionTypeId.Of<long>(); Long = value; }
        public Union(ulong value)  : this() { TypeKind = UnionTypeKind.ULong ; TypeId = UnionTypeId.Of<ulong>(); ULong = value; }
        public Union(short value)  : this() { TypeKind = UnionTypeKind.Short ; TypeId = UnionTypeId.Of<short>(); Short = value; }
        public Union(ushort value) : this() { TypeKind = UnionTypeKind.UShort; TypeId = UnionTypeId.Of<ushort>(); UShort = value; }
        public Union(string value) : this() { TypeKind = UnionTypeKind.String; TypeId = UnionTypeId.Of<string>(); GCHandle = GCHandle.Alloc(value, GCHandleType.Weak); }
        public Union(object value) : this() { TypeKind = UnionTypeKind.Object; TypeId = UnionTypeId.Of<object>(); GCHandle = GCHandle.Alloc(value, GCHandleType.Weak); }
        
        public Union(UnionTypeId typeId, object value) : this()
        {
            TypeKind = UnionTypeKind.Object;
            TypeId = typeId;
            GCHandle = GCHandle.Alloc(value, GCHandleType.Weak);
        }

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

        public void Dispose()
        {
            if (GCHandle.IsAllocated)
            {
                GCHandle.Free();
            }
        }

        public bool TypeEquals(in Union other)
            => TypeKind == other.TypeKind;

        public bool TryGetValue(out bool result)
        {
            if (TypeKind == UnionTypeKind.Bool)
            {
                result = Bool; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out byte result)
        {
            if (TypeKind == UnionTypeKind.Byte)
            {
                result = Byte; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out sbyte result)
        {
            if (TypeKind == UnionTypeKind.SByte)
            {
                result = SByte; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out char result)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.SByte: result = (char)SByte; return true;
                case UnionTypeKind.Char: result = Char; return true;
                case UnionTypeKind.UShort: result = (char)UShort; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out double result)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: result = Byte; return true;
                case UnionTypeKind.SByte: result = SByte; return true;
                case UnionTypeKind.Char: result = Char; return true;
                case UnionTypeKind.Double: result = Double; return true;
                case UnionTypeKind.Float: result = Float; return true;
                case UnionTypeKind.Int: result = Int; return true;
                case UnionTypeKind.UInt: result = UInt; return true;
                case UnionTypeKind.Long: result = Long; return true;
                case UnionTypeKind.ULong: result = ULong; return true;
                case UnionTypeKind.Short: result = Short; return true;
                case UnionTypeKind.UShort: result = UShort; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out float result)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: result = Byte; return true;
                case UnionTypeKind.SByte: result = SByte; return true;
                case UnionTypeKind.Char: result = Char; return true;
                case UnionTypeKind.Float: result = Float; return true;
                case UnionTypeKind.Int: result = Int; return true;
                case UnionTypeKind.UInt: result = UInt; return true;
                case UnionTypeKind.Long: result = Long; return true;
                case UnionTypeKind.ULong: result = ULong; return true;
                case UnionTypeKind.Short: result = Short; return true;
                case UnionTypeKind.UShort: result = UShort; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out int result)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: result = Byte; return true;
                case UnionTypeKind.SByte: result = SByte; return true;
                case UnionTypeKind.Char: result = Char; return true;
                case UnionTypeKind.Int: result = Int; return true;
                case UnionTypeKind.Short: result = Short; return true;
                case UnionTypeKind.UShort: result = UShort; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out uint result)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: result = Byte; return true;
                case UnionTypeKind.Char: result = Char; return true;
                case UnionTypeKind.UInt: result = UInt; return true;
                case UnionTypeKind.UShort: result = UShort; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out long result)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: result = Byte; return true;
                case UnionTypeKind.SByte: result = SByte; return true;
                case UnionTypeKind.Char: result = Char; return true;
                case UnionTypeKind.Int: result = Int; return true;
                case UnionTypeKind.UInt: result = UInt; return true;
                case UnionTypeKind.Long: result = Long; return true;
                case UnionTypeKind.Short: result = Short; return true;
                case UnionTypeKind.UShort: result = UShort; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out ulong result)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: result = Byte; return true;
                case UnionTypeKind.Char: result = Char; return true;
                case UnionTypeKind.UInt: result = UInt; return true;
                case UnionTypeKind.ULong: result = ULong; return true;
                case UnionTypeKind.UShort: result = UShort; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out short result)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: result = Byte; return true;
                case UnionTypeKind.SByte: result = SByte; return true;
                case UnionTypeKind.Short: result = Short; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out ushort result)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: result = Byte; return true;
                case UnionTypeKind.Char: result = Char; return true;
                case UnionTypeKind.UShort: result = UShort; return true;
            }

            result = default; return false;
        }

        public bool TryGetValue(out string result)
        {
            if (TypeKind == UnionTypeKind.String && GCHandle.IsAllocated == true)
            {
                result = GCHandle.Target as string;
                return true;
            }

            result = default;
            return false;
        }

        public bool TryGetValue(out object result)
        {
            if (TypeKind == UnionTypeKind.Object && GCHandle.IsAllocated == true)
            {
                result = GCHandle.Target;
                return true;
            }

            result = default;
            return false;
        }

        public bool TrySetValueTo(ref bool dest)
        {
            if (TypeKind == UnionTypeKind.Bool)
            {
                dest = Bool; return true;
            }

            return false;
        }

        public bool TrySetValueTo(ref byte dest)
        {
            if (TypeKind == UnionTypeKind.Byte)
            {
                dest = Byte; return true;
            }

            return false;
        }

        public bool TrySetValueTo(ref sbyte dest)
        {
            if (TypeKind == UnionTypeKind.SByte)
            {
                dest = SByte; return true;
            }

            return false;
        }

        public bool TrySetValueTo(ref char dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.SByte: dest = (char)SByte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.UShort: dest = (char)UShort; return true;
            }

            return false;
        }

        public bool TrySetValueTo(ref double dest)
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

        public bool TrySetValueTo(ref float dest)
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

        public bool TrySetValueTo(ref int dest)
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

        public bool TrySetValueTo(ref uint dest)
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

        public bool TrySetValueTo(ref long dest)
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

        public bool TrySetValueTo(ref ulong dest)
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

        public bool TrySetValueTo(ref short dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.SByte: dest = SByte; return true;
                case UnionTypeKind.Short: dest = Short; return true;
            }

            return false;
        }

        public bool TrySetValueTo(ref ushort dest)
        {
            switch (TypeKind)
            {
                case UnionTypeKind.Byte: dest = Byte; return true;
                case UnionTypeKind.Char: dest = Char; return true;
                case UnionTypeKind.UShort: dest = UShort; return true;
            }

            return false;
        }

        public bool TrySetValueTo(ref string dest)
        {
            if (TypeKind == UnionTypeKind.String && GCHandle.IsAllocated == true)
            {
                dest = GCHandle.Target as string;
                return true;
            }

            return false;
        }

        public bool TrySetValueTo(ref object dest)
        {
            if (TypeKind == UnionTypeKind.Object && GCHandle.IsAllocated == true)
            {
                dest = GCHandle.Target;
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

                case UnionTypeKind.String:
                {
                    if (GCHandle.IsAllocated == true && GCHandle.Target is string value)
                    {
                        return value;
                    }

                    return string.Empty;
                }

                case UnionTypeKind.Object:
                {
                    if (GCHandle.IsAllocated == true && GCHandle.Target is object value)
                    {
                        return value.ToString();
                    }

                    return TypeId.AsType()?.ToString() ?? string.Empty;
                }

                case UnionTypeKind.ValueType: return TypeId.AsType()?.ToString() ?? string.Empty;
            }

            if (TypeId != UnionTypeId.Undefined)
            {
                return $"{UnionTypeKind.Undefined}: {TypeId.AsType()}";
            }

            return string.Empty;
        }
    }
}
