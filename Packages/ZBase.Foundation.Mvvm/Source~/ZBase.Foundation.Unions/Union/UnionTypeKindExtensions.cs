namespace ZBase.Foundation.Unions
{
    public static class UnionTypeKindExtensions
    {
        public static UnionTypeKind ToUnionType(this string value)
            => value switch {
                "bool" => UnionTypeKind.Bool,
                "byte" => UnionTypeKind.Byte,
                "sbyte" => UnionTypeKind.SByte,
                "char" => UnionTypeKind.Char,
                "double" => UnionTypeKind.Double,
                "float" => UnionTypeKind.Float,
                "int" => UnionTypeKind.Int,
                "uint" => UnionTypeKind.UInt,
                "long" => UnionTypeKind.Long,
                "ulong" => UnionTypeKind.ULong,
                "short" => UnionTypeKind.Short,
                "ushort" => UnionTypeKind.UShort,
                "string" => UnionTypeKind.String,
                "object" => UnionTypeKind.Object,
                nameof(System.Boolean) => UnionTypeKind.Bool,
                nameof(System.Byte) => UnionTypeKind.Byte,
                nameof(System.SByte) => UnionTypeKind.SByte,
                nameof(System.Char) => UnionTypeKind.Char,
                nameof(System.Double) => UnionTypeKind.Double,
                nameof(System.Single) => UnionTypeKind.Float,
                nameof(System.Int32) => UnionTypeKind.Int,
                nameof(System.UInt32) => UnionTypeKind.UInt,
                nameof(System.Int64) => UnionTypeKind.Long,
                nameof(System.UInt64) => UnionTypeKind.ULong,
                nameof(System.Int16) => UnionTypeKind.Short,
                nameof(System.UInt16) => UnionTypeKind.UShort,
                nameof(System.String) => UnionTypeKind.String,
                nameof(System.Object) => UnionTypeKind.Object,
                not null and string { Length: >= 0 } => UnionTypeKind.UserDefined,
                _ => UnionTypeKind.Undefined,
            };

        public static bool IsNativeUnionType(this string value)
            => value.ToUnionType() switch {
                UnionTypeKind.Bool => true,
                UnionTypeKind.Byte => true,
                UnionTypeKind.SByte => true,
                UnionTypeKind.Char => true,
                UnionTypeKind.Double => true,
                UnionTypeKind.Float => true,
                UnionTypeKind.Int => true,
                UnionTypeKind.UInt => true,
                UnionTypeKind.Long => true,
                UnionTypeKind.ULong => true,
                UnionTypeKind.Short => true,
                UnionTypeKind.UShort => true,
                UnionTypeKind.String => true,
                UnionTypeKind.Object => true,
                _ => false,
            };

        public static bool IsEnumUnderlyingType(this UnionTypeKind value)
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

        public static UnionTypeKind ToEnumUnderlyingTypeKind<TEnum>(this TEnum value)
            where TEnum : unmanaged, System.Enum
            => value.GetTypeCode() switch {
                System.TypeCode.Byte => UnionTypeKind.Byte,
                System.TypeCode.SByte => UnionTypeKind.SByte,
                System.TypeCode.Int32 => UnionTypeKind.Int,
                System.TypeCode.UInt32 => UnionTypeKind.UInt,
                System.TypeCode.Int64 => UnionTypeKind.Long,
                System.TypeCode.UInt64 => UnionTypeKind.ULong,
                System.TypeCode.Int16 => UnionTypeKind.Short,
                System.TypeCode.UInt16 => UnionTypeKind.UShort,
                _ => UnionTypeKind.Undefined,
            };

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
