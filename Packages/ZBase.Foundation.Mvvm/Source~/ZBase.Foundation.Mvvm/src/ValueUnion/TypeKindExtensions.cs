namespace ZBase.Foundation.Mvvm
{
    public static class TypeKindExtensions
    {
        public static TypeKind ToTypeKind(this System.TypeCode value)
            => value switch {
                System.TypeCode.Boolean => TypeKind.Bool,
                System.TypeCode.Byte => TypeKind.Byte,
                System.TypeCode.SByte => TypeKind.SByte,
                System.TypeCode.Char => TypeKind.Char,
                System.TypeCode.Double => TypeKind.Double,
                System.TypeCode.Single => TypeKind.Float,
                System.TypeCode.Int32 => TypeKind.Int,
                System.TypeCode.UInt32 => TypeKind.UInt,
                System.TypeCode.Int64 => TypeKind.Long,
                System.TypeCode.UInt64 => TypeKind.ULong,
                System.TypeCode.Int16 => TypeKind.Short,
                System.TypeCode.UInt16 => TypeKind.UShort,
                System.TypeCode.String => TypeKind.String,
                System.TypeCode.Object => TypeKind.Object,
                _ => throw new System.InvalidCastException("Not a valid supported type."),
            };

        public static TypeKind ToEnumTypeKind(this TypeKind value)
            => value switch {
                TypeKind.Byte => TypeKind.Byte,
                TypeKind.SByte => TypeKind.SByte,
                TypeKind.Int => TypeKind.Int,
                TypeKind.UInt => TypeKind.UInt,
                TypeKind.Long => TypeKind.Long,
                TypeKind.ULong => TypeKind.ULong,
                TypeKind.Short => TypeKind.Short,
                TypeKind.UShort => TypeKind.UShort,
                _ => throw new System.InvalidCastException("Not a valid supported type for enum."),
            };

        public static TypeKind ToEnumTypeKind(this System.TypeCode value)
            => value switch {
                System.TypeCode.Byte => TypeKind.Byte,
                System.TypeCode.SByte => TypeKind.SByte,
                System.TypeCode.Int32 => TypeKind.Int,
                System.TypeCode.UInt32 => TypeKind.UInt,
                System.TypeCode.Int64 => TypeKind.Long,
                System.TypeCode.UInt64 => TypeKind.ULong,
                System.TypeCode.Int16 => TypeKind.Short,
                System.TypeCode.UInt16 => TypeKind.UShort,
                _ => throw new System.InvalidCastException("Not a valid supported type for enum."),
            };

        public static bool IsEnumTypeKind(this System.TypeCode value)
            => value switch {
                System.TypeCode.Byte => true,
                System.TypeCode.SByte => true,
                System.TypeCode.Int32 => true,
                System.TypeCode.UInt32 => true,
                System.TypeCode.Int64 => true,
                System.TypeCode.UInt64 => true,
                System.TypeCode.Int16 => true,
                System.TypeCode.UInt16 => true,
                _ => false,
            };

        public static bool IsEnumTypeKind(this TypeKind value)
            => value switch {
                TypeKind.Byte => true,
                TypeKind.SByte => true,
                TypeKind.Int => true,
                TypeKind.UInt => true,
                TypeKind.Long => true,
                TypeKind.ULong => true,
                TypeKind.Short => true,
                TypeKind.UShort => true,
                _ => false,
            };

        public static TypeKind ToEnumTypeKind<TEnum>(this TEnum value)
            where TEnum : unmanaged, System.Enum
            => value.GetTypeCode().ToEnumTypeKind();

        public static bool IsNumberImplicitlyConvertible(this TypeKind src, TypeKind dest)
        {
            switch (src)
            {
                case TypeKind.Byte:
                {
                    return dest switch {
                        TypeKind.Byte => true,
                        TypeKind.Short => true,
                        TypeKind.UShort => true,
                        TypeKind.Int => true,
                        TypeKind.UInt => true,
                        TypeKind.Long => true,
                        TypeKind.ULong => true,
                        TypeKind.Float => true,
                        TypeKind.Double => true,
                        _ => false,
                    };
                }

                case TypeKind.SByte:
                {
                    return dest switch {
                        TypeKind.SByte => true,
                        TypeKind.Short => true,
                        TypeKind.Int => true,
                        TypeKind.Long => true,
                        TypeKind.Float => true,
                        TypeKind.Double => true,
                        _ => false,
                    };
                }

                case TypeKind.Double:
                {
                    return dest switch {
                        TypeKind.Double => true,
                        _ => false,
                    };
                }

                case TypeKind.Float:
                {
                    return dest switch {
                        TypeKind.Float => true,
                        TypeKind.Double => true,
                        _ => false,
                    };
                }

                case TypeKind.Int:
                {
                    return dest switch {
                        TypeKind.Int => true,
                        TypeKind.Long => true,
                        TypeKind.Float => true,
                        TypeKind.Double => true,
                        _ => false,
                    };
                }

                case TypeKind.UInt:
                {
                    return dest switch {
                        TypeKind.UInt => true,
                        TypeKind.Long => true,
                        TypeKind.ULong => true,
                        TypeKind.Float => true,
                        TypeKind.Double => true,
                        _ => false,
                    };
                }

                case TypeKind.Long:
                {
                    return dest switch {
                        TypeKind.Long => true,
                        TypeKind.Float => true,
                        TypeKind.Double => true,
                        _ => false,
                    };
                }

                case TypeKind.ULong:
                {
                    return dest switch {
                        TypeKind.ULong => true,
                        TypeKind.Float => true,
                        TypeKind.Double => true,
                        _ => false,
                    };
                }

                case TypeKind.Short:
                {
                    return dest switch {
                        TypeKind.Short => true,
                        TypeKind.Int => true,
                        TypeKind.Long => true,
                        TypeKind.Float => true,
                        TypeKind.Double => true,
                        _ => false,
                    };
                }

                case TypeKind.UShort:
                {
                    return dest switch {
                        TypeKind.UShort => true,
                        TypeKind.Int => true,
                        TypeKind.UInt => true,
                        TypeKind.Long => true,
                        TypeKind.ULong => true,
                        TypeKind.Float => true,
                        TypeKind.Double => true,
                        _ => false,
                    };
                }
            }

            return false;
        }
    }
}
