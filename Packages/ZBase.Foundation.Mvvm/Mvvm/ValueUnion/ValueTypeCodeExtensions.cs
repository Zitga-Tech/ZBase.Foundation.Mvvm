using System;

namespace ZBase.Foundation.Mvvm
{
    public static class ValueTypeCodeExtensions
    {
        public static ValueTypeCode ToValueTypeCode(this TypeCode value)
            => value switch {
                TypeCode.Boolean => ValueTypeCode.Bool,
                TypeCode.Byte    => ValueTypeCode.Byte,
                TypeCode.SByte   => ValueTypeCode.SByte,
                TypeCode.Char    => ValueTypeCode.Char,
                TypeCode.Double  => ValueTypeCode.Double,
                TypeCode.Single  => ValueTypeCode.Float,
                TypeCode.Int32   => ValueTypeCode.Int,
                TypeCode.UInt32  => ValueTypeCode.UInt,
                TypeCode.Int64   => ValueTypeCode.Long,
                TypeCode.UInt64  => ValueTypeCode.ULong,
                TypeCode.Int16   => ValueTypeCode.Short,
                TypeCode.UInt16  => ValueTypeCode.UShort,
                TypeCode.String  => ValueTypeCode.String,
                TypeCode.Object  => ValueTypeCode.Object,
                _                => throw new InvalidCastException("Not a valid supported type."),
            };

        public static ValueTypeCode ToEnumTypeCode(this ValueTypeCode value)
            => value switch {
                ValueTypeCode.Byte   => ValueTypeCode.Byte,
                ValueTypeCode.SByte  => ValueTypeCode.SByte,
                ValueTypeCode.Int    => ValueTypeCode.Int,
                ValueTypeCode.UInt   => ValueTypeCode.UInt,
                ValueTypeCode.Long   => ValueTypeCode.Long,
                ValueTypeCode.ULong  => ValueTypeCode.ULong,
                ValueTypeCode.Short  => ValueTypeCode.Short,
                ValueTypeCode.UShort => ValueTypeCode.UShort,
                _               => throw new InvalidCastException("Not a valid supported type for enum."),
            };

        public static ValueTypeCode ToEnumTypeCode(this TypeCode value)
            => value switch {
                TypeCode.Byte    => ValueTypeCode.Byte,
                TypeCode.SByte   => ValueTypeCode.SByte,
                TypeCode.Int32   => ValueTypeCode.Int,
                TypeCode.UInt32  => ValueTypeCode.UInt,
                TypeCode.Int64   => ValueTypeCode.Long,
                TypeCode.UInt64  => ValueTypeCode.ULong,
                TypeCode.Int16   => ValueTypeCode.Short,
                TypeCode.UInt16  => ValueTypeCode.UShort,
                _                => throw new InvalidCastException("Not a valid supported type for enum."),
            };

        public static bool IsEnumTypeCode(this TypeCode value)
            => value switch {
                TypeCode.Byte   => true,
                TypeCode.SByte  => true,
                TypeCode.Int32  => true,
                TypeCode.UInt32 => true,
                TypeCode.Int64  => true,
                TypeCode.UInt64 => true,
                TypeCode.Int16  => true,
                TypeCode.UInt16 => true,
                _               => false,
            };

        public static bool IsEnumTypeCode(this ValueTypeCode value)
            => value switch {
                ValueTypeCode.Byte   => true,
                ValueTypeCode.SByte  => true,
                ValueTypeCode.Int    => true,
                ValueTypeCode.UInt   => true,
                ValueTypeCode.Long   => true,
                ValueTypeCode.ULong  => true,
                ValueTypeCode.Short  => true,
                ValueTypeCode.UShort => true,
                _               => false,
            };

        public static ValueTypeCode ToEnumTypeCode<TEnum>(this TEnum value)
            where TEnum : unmanaged, System.Enum
            => value.GetTypeCode().ToEnumTypeCode();

        public static bool IsNumberImplicitlyConvertible(this ValueTypeCode src, ValueTypeCode dest)
        {
            switch (src)
            {
                case ValueTypeCode.Byte:
                {
                    return dest switch {
                        ValueTypeCode.Byte   => true,
                        ValueTypeCode.Short  => true,
                        ValueTypeCode.UShort => true,
                        ValueTypeCode.Int    => true,
                        ValueTypeCode.UInt   => true,
                        ValueTypeCode.Long   => true,
                        ValueTypeCode.ULong  => true,
                        ValueTypeCode.Float  => true,
                        ValueTypeCode.Double => true,
                        _ => false,
                    };
                }

                case ValueTypeCode.SByte:
                {
                    return dest switch {
                        ValueTypeCode.SByte  => true,
                        ValueTypeCode.Short  => true,
                        ValueTypeCode.Int    => true,
                        ValueTypeCode.Long   => true,
                        ValueTypeCode.Float  => true,
                        ValueTypeCode.Double => true,
                        _ => false,
                    };
                }    

                case ValueTypeCode.Double:
                {
                    return dest switch {
                        ValueTypeCode.Double => true,
                        _ => false,
                    };
                }    

                case ValueTypeCode.Float:
                {
                    return dest switch {
                        ValueTypeCode.Float  => true,
                        ValueTypeCode.Double => true,
                        _ => false,
                    };
                }

                case ValueTypeCode.Int:
                {
                    return dest switch {
                        ValueTypeCode.Int    => true,
                        ValueTypeCode.Long   => true,
                        ValueTypeCode.Float  => true,
                        ValueTypeCode.Double => true,
                        _ => false,
                    };
                }

                case ValueTypeCode.UInt:
                {
                    return dest switch {
                        ValueTypeCode.UInt   => true,
                        ValueTypeCode.Long   => true,
                        ValueTypeCode.ULong  => true,
                        ValueTypeCode.Float  => true,
                        ValueTypeCode.Double => true,
                        _ => false,
                    };
                }

                case ValueTypeCode.Long:
                {
                    return dest switch {
                        ValueTypeCode.Long   => true,
                        ValueTypeCode.Float  => true,
                        ValueTypeCode.Double => true,
                        _ => false,
                    };
                }

                case ValueTypeCode.ULong:
                {
                    return dest switch {
                        ValueTypeCode.ULong  => true,
                        ValueTypeCode.Float  => true,
                        ValueTypeCode.Double => true,
                        _ => false,
                    };
                }

                case ValueTypeCode.Short:
                {
                    return dest switch {
                        ValueTypeCode.Short  => true,
                        ValueTypeCode.Int    => true,
                        ValueTypeCode.Long   => true,
                        ValueTypeCode.Float  => true,
                        ValueTypeCode.Double => true,
                        _ => false,
                    };
                }

                case ValueTypeCode.UShort:
                {
                    return dest switch {
                        ValueTypeCode.UShort => true,
                        ValueTypeCode.Int    => true,
                        ValueTypeCode.UInt   => true,
                        ValueTypeCode.Long   => true,
                        ValueTypeCode.ULong  => true,
                        ValueTypeCode.Float  => true,
                        ValueTypeCode.Double => true,
                        _ => false,
                    };
                }    
            }

            return false;
        }
    }
}
