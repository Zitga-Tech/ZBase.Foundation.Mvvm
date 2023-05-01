namespace ZBase.Foundation.Unions
{
    public static class UnionTypeKindExtensions
    {
        public static UnionTypeKind ToUnionTypeKind(this System.TypeCode value)
            => value switch {
                System.TypeCode.Boolean => UnionTypeKind.Bool,
                System.TypeCode.Byte => UnionTypeKind.Byte,
                System.TypeCode.SByte => UnionTypeKind.SByte,
                System.TypeCode.Char => UnionTypeKind.Char,
                System.TypeCode.Double => UnionTypeKind.Double,
                System.TypeCode.Single => UnionTypeKind.Float,
                System.TypeCode.Int32 => UnionTypeKind.Int,
                System.TypeCode.UInt32 => UnionTypeKind.UInt,
                System.TypeCode.Int64 => UnionTypeKind.Long,
                System.TypeCode.UInt64 => UnionTypeKind.ULong,
                System.TypeCode.Int16 => UnionTypeKind.Short,
                System.TypeCode.UInt16 => UnionTypeKind.UShort,
                System.TypeCode.String => UnionTypeKind.String,
                System.TypeCode.Object => UnionTypeKind.Object,
                _ => throw new System.InvalidCastException("Not a valid supported type."),
            };

        public static UnionTypeKind ToEnumTypeKind(this UnionTypeKind value)
            => value switch {
                UnionTypeKind.Byte => UnionTypeKind.Byte,
                UnionTypeKind.SByte => UnionTypeKind.SByte,
                UnionTypeKind.Int => UnionTypeKind.Int,
                UnionTypeKind.UInt => UnionTypeKind.UInt,
                UnionTypeKind.Long => UnionTypeKind.Long,
                UnionTypeKind.ULong => UnionTypeKind.ULong,
                UnionTypeKind.Short => UnionTypeKind.Short,
                UnionTypeKind.UShort => UnionTypeKind.UShort,
                _ => throw new System.InvalidCastException("Not a valid supported type for enum."),
            };

        public static UnionTypeKind ToEnumTypeKind(this System.TypeCode value)
            => value switch {
                System.TypeCode.Byte => UnionTypeKind.Byte,
                System.TypeCode.SByte => UnionTypeKind.SByte,
                System.TypeCode.Int32 => UnionTypeKind.Int,
                System.TypeCode.UInt32 => UnionTypeKind.UInt,
                System.TypeCode.Int64 => UnionTypeKind.Long,
                System.TypeCode.UInt64 => UnionTypeKind.ULong,
                System.TypeCode.Int16 => UnionTypeKind.Short,
                System.TypeCode.UInt16 => UnionTypeKind.UShort,
                _ => throw new System.InvalidCastException("Not a valid supported type for enum."),
            };

        public static bool IsEnumUnionTypeKind(this UnionTypeKind value)
            => value switch {
                UnionTypeKind.Byte => true,
                UnionTypeKind.SByte => true,
                UnionTypeKind.Int => true,
                UnionTypeKind.UInt => true,
                UnionTypeKind.Long => true,
                UnionTypeKind.ULong => true,
                UnionTypeKind.Short => true,
                UnionTypeKind.UShort => true,
                _ => false,
            };

        public static UnionTypeKind ToEnumTypeKind<TEnum>(this TEnum value)
            where TEnum : unmanaged, System.Enum
            => value.GetTypeCode().ToEnumTypeKind();

        public static bool IsNumberImplicitlyConvertible(this UnionTypeKind src, UnionTypeKind dest)
        {
            switch (src)
            {
                case UnionTypeKind.Byte:
                {
                    return dest switch {
                        UnionTypeKind.Byte => true,
                        UnionTypeKind.Short => true,
                        UnionTypeKind.UShort => true,
                        UnionTypeKind.Int => true,
                        UnionTypeKind.UInt => true,
                        UnionTypeKind.Long => true,
                        UnionTypeKind.ULong => true,
                        UnionTypeKind.Float => true,
                        UnionTypeKind.Double => true,
                        _ => false,
                    };
                }

                case UnionTypeKind.SByte:
                {
                    return dest switch {
                        UnionTypeKind.SByte => true,
                        UnionTypeKind.Short => true,
                        UnionTypeKind.Int => true,
                        UnionTypeKind.Long => true,
                        UnionTypeKind.Float => true,
                        UnionTypeKind.Double => true,
                        _ => false,
                    };
                }

                case UnionTypeKind.Double:
                {
                    return dest switch {
                        UnionTypeKind.Double => true,
                        _ => false,
                    };
                }

                case UnionTypeKind.Float:
                {
                    return dest switch {
                        UnionTypeKind.Float => true,
                        UnionTypeKind.Double => true,
                        _ => false,
                    };
                }

                case UnionTypeKind.Int:
                {
                    return dest switch {
                        UnionTypeKind.Int => true,
                        UnionTypeKind.Long => true,
                        UnionTypeKind.Float => true,
                        UnionTypeKind.Double => true,
                        _ => false,
                    };
                }

                case UnionTypeKind.UInt:
                {
                    return dest switch {
                        UnionTypeKind.UInt => true,
                        UnionTypeKind.Long => true,
                        UnionTypeKind.ULong => true,
                        UnionTypeKind.Float => true,
                        UnionTypeKind.Double => true,
                        _ => false,
                    };
                }

                case UnionTypeKind.Long:
                {
                    return dest switch {
                        UnionTypeKind.Long => true,
                        UnionTypeKind.Float => true,
                        UnionTypeKind.Double => true,
                        _ => false,
                    };
                }

                case UnionTypeKind.ULong:
                {
                    return dest switch {
                        UnionTypeKind.ULong => true,
                        UnionTypeKind.Float => true,
                        UnionTypeKind.Double => true,
                        _ => false,
                    };
                }

                case UnionTypeKind.Short:
                {
                    return dest switch {
                        UnionTypeKind.Short => true,
                        UnionTypeKind.Int => true,
                        UnionTypeKind.Long => true,
                        UnionTypeKind.Float => true,
                        UnionTypeKind.Double => true,
                        _ => false,
                    };
                }

                case UnionTypeKind.UShort:
                {
                    return dest switch {
                        UnionTypeKind.UShort => true,
                        UnionTypeKind.Int => true,
                        UnionTypeKind.UInt => true,
                        UnionTypeKind.Long => true,
                        UnionTypeKind.ULong => true,
                        UnionTypeKind.Float => true,
                        UnionTypeKind.Double => true,
                        _ => false,
                    };
                }
            }

            return false;
        }
    }
}
