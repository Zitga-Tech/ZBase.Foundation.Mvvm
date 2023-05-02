//using System;
//using System.Runtime.CompilerServices;

//namespace ZBase.Foundation.Unions
//{
//    public static class UnionTypeKindExtensions
//    {
//        public static bool IsNativeUnionType(this UnionTypeKind value)
//            => value switch {
//                UnionTypeKind.Bool => true,
//                UnionTypeKind.Byte => true,
//                UnionTypeKind.SByte => true,
//                UnionTypeKind.Char => true,
//                UnionTypeKind.Double => true,
//                UnionTypeKind.Float => true,
//                UnionTypeKind.Int => true,
//                UnionTypeKind.UInt => true,
//                UnionTypeKind.Long => true,
//                UnionTypeKind.ULong => true,
//                UnionTypeKind.Short => true,
//                UnionTypeKind.UShort => true,
//                UnionTypeKind.String => true,
//                UnionTypeKind.Object => true,
//                _ => false,
//            };

//        public static UnionTypeKind GetUnionType<T>(this T _)
//        {
//            var type = typeof(T);
//            var typeCode = Type.GetTypeCode(type);

//            return typeCode switch {
//                TypeCode.Boolean => UnionTypeKind.Bool,
//                TypeCode.Byte => UnionTypeKind.Byte,
//                TypeCode.SByte => UnionTypeKind.SByte,
//                TypeCode.Char => UnionTypeKind.Char,
//                TypeCode.Double => UnionTypeKind.Double,
//                TypeCode.Single => UnionTypeKind.Float,
//                TypeCode.Int32 => UnionTypeKind.Int,
//                TypeCode.UInt32 => UnionTypeKind.UInt,
//                TypeCode.Int64 => UnionTypeKind.Long,
//                TypeCode.UInt64 => UnionTypeKind.ULong,
//                TypeCode.Int16 => UnionTypeKind.Short,
//                TypeCode.UInt16 => UnionTypeKind.UShort,
//                TypeCode.String => UnionTypeKind.String,

//                TypeCode.Object => type.IsValueType
//                    ? UnionTypeKind.ValueType
//                    : UnionTypeKind.Object,

//                _ => UnionTypeKind.Undefined,
//            };
//        }

//        [MethodImpl(MethodImplOptions.AggressiveInlining)]
//        public static bool IsNativeUnionType(this string value)
//            => IsNativeUnionType(value.ToUnionType());

//        public static UnionTypeKind ToUnionType(this string value)
//            => value switch {
//                "bool" => UnionTypeKind.Bool,
//                "byte" => UnionTypeKind.Byte,
//                "sbyte" => UnionTypeKind.SByte,
//                "char" => UnionTypeKind.Char,
//                "double" => UnionTypeKind.Double,
//                "float" => UnionTypeKind.Float,
//                "int" => UnionTypeKind.Int,
//                "uint" => UnionTypeKind.UInt,
//                "long" => UnionTypeKind.Long,
//                "ulong" => UnionTypeKind.ULong,
//                "short" => UnionTypeKind.Short,
//                "ushort" => UnionTypeKind.UShort,
//                "string" => UnionTypeKind.String,
//                "object" => UnionTypeKind.Object,
//                "Boolean" => UnionTypeKind.Bool,
//                "Byte" => UnionTypeKind.Byte,
//                "SByte" => UnionTypeKind.SByte,
//                "Char" => UnionTypeKind.Char,
//                "Double" => UnionTypeKind.Double,
//                "Single" => UnionTypeKind.Float,
//                "Int32" => UnionTypeKind.Int,
//                "UInt32" => UnionTypeKind.UInt,
//                "Int64" => UnionTypeKind.Long,
//                "UInt64" => UnionTypeKind.ULong,
//                "Int16" => UnionTypeKind.Short,
//                "UInt16" => UnionTypeKind.UShort,
//                "String" => UnionTypeKind.String,
//                "Object" => UnionTypeKind.Object,
//                "System.Boolean" => UnionTypeKind.Bool,
//                "System.Byte" => UnionTypeKind.Byte,
//                "System.SByte" => UnionTypeKind.SByte,
//                "System.Char" => UnionTypeKind.Char,
//                "System.Double" => UnionTypeKind.Double,
//                "System.Single" => UnionTypeKind.Float,
//                "System.Int32" => UnionTypeKind.Int,
//                "System.UInt32" => UnionTypeKind.UInt,
//                "System.Int64" => UnionTypeKind.Long,
//                "System.UInt64" => UnionTypeKind.ULong,
//                "System.Int16" => UnionTypeKind.Short,
//                "System.UInt16" => UnionTypeKind.UShort,
//                "System.String" => UnionTypeKind.String,
//                "System.Object" => UnionTypeKind.Object,
//                "global::System.Boolean" => UnionTypeKind.Bool,
//                "global::System.Byte" => UnionTypeKind.Byte,
//                "global::System.SByte" => UnionTypeKind.SByte,
//                "global::System.Char" => UnionTypeKind.Char,
//                "global::System.Double" => UnionTypeKind.Double,
//                "global::System.Single" => UnionTypeKind.Float,
//                "global::System.Int32" => UnionTypeKind.Int,
//                "global::System.UInt32" => UnionTypeKind.UInt,
//                "global::System.Int64" => UnionTypeKind.Long,
//                "global::System.UInt64" => UnionTypeKind.ULong,
//                "global::System.Int16" => UnionTypeKind.Short,
//                "global::System.UInt16" => UnionTypeKind.UShort,
//                "global::System.String" => UnionTypeKind.String,
//                "global::System.Object" => UnionTypeKind.Object,
//                not null and string { Length: >= 0 } => UnionTypeKind.Object,
//                _ => UnionTypeKind.Undefined,
//            };

//        public static bool IsEnumUnderlyingType(this UnionTypeKind value)
//            => value switch {
//                UnionTypeKind.Byte => true,
//                UnionTypeKind.SByte => true,
//                UnionTypeKind.Int => true,
//                UnionTypeKind.UInt => true,
//                UnionTypeKind.Long => true,
//                UnionTypeKind.ULong => true,
//                UnionTypeKind.Short => true,
//                UnionTypeKind.UShort => true,
//                _ => false,
//            };

//        public static UnionTypeKind GetEnumUnderlyingType<TEnum>(this TEnum value)
//            where TEnum : unmanaged, Enum
//            => value.GetTypeCode() switch {
//                TypeCode.Byte => UnionTypeKind.Byte,
//                TypeCode.SByte => UnionTypeKind.SByte,
//                TypeCode.Int32 => UnionTypeKind.Int,
//                TypeCode.UInt32 => UnionTypeKind.UInt,
//                TypeCode.Int64 => UnionTypeKind.Long,
//                TypeCode.UInt64 => UnionTypeKind.ULong,
//                TypeCode.Int16 => UnionTypeKind.Short,
//                TypeCode.UInt16 => UnionTypeKind.UShort,
//                _ => UnionTypeKind.Undefined,
//            };

//        public static bool IsNumberImplicitlyConvertible(this UnionTypeKind src, UnionTypeKind dest)
//        {
//            switch (src)
//            {
//                case UnionTypeKind.Byte:
//                {
//                    return dest switch {
//                        UnionTypeKind.Byte => true,
//                        UnionTypeKind.Short => true,
//                        UnionTypeKind.UShort => true,
//                        UnionTypeKind.Int => true,
//                        UnionTypeKind.UInt => true,
//                        UnionTypeKind.Long => true,
//                        UnionTypeKind.ULong => true,
//                        UnionTypeKind.Float => true,
//                        UnionTypeKind.Double => true,
//                        _ => false,
//                    };
//                }

//                case UnionTypeKind.SByte:
//                {
//                    return dest switch {
//                        UnionTypeKind.SByte => true,
//                        UnionTypeKind.Short => true,
//                        UnionTypeKind.Int => true,
//                        UnionTypeKind.Long => true,
//                        UnionTypeKind.Float => true,
//                        UnionTypeKind.Double => true,
//                        _ => false,
//                    };
//                }

//                case UnionTypeKind.Double:
//                {
//                    return dest switch {
//                        UnionTypeKind.Double => true,
//                        _ => false,
//                    };
//                }

//                case UnionTypeKind.Float:
//                {
//                    return dest switch {
//                        UnionTypeKind.Float => true,
//                        UnionTypeKind.Double => true,
//                        _ => false,
//                    };
//                }

//                case UnionTypeKind.Int:
//                {
//                    return dest switch {
//                        UnionTypeKind.Int => true,
//                        UnionTypeKind.Long => true,
//                        UnionTypeKind.Float => true,
//                        UnionTypeKind.Double => true,
//                        _ => false,
//                    };
//                }

//                case UnionTypeKind.UInt:
//                {
//                    return dest switch {
//                        UnionTypeKind.UInt => true,
//                        UnionTypeKind.Long => true,
//                        UnionTypeKind.ULong => true,
//                        UnionTypeKind.Float => true,
//                        UnionTypeKind.Double => true,
//                        _ => false,
//                    };
//                }

//                case UnionTypeKind.Long:
//                {
//                    return dest switch {
//                        UnionTypeKind.Long => true,
//                        UnionTypeKind.Float => true,
//                        UnionTypeKind.Double => true,
//                        _ => false,
//                    };
//                }

//                case UnionTypeKind.ULong:
//                {
//                    return dest switch {
//                        UnionTypeKind.ULong => true,
//                        UnionTypeKind.Float => true,
//                        UnionTypeKind.Double => true,
//                        _ => false,
//                    };
//                }

//                case UnionTypeKind.Short:
//                {
//                    return dest switch {
//                        UnionTypeKind.Short => true,
//                        UnionTypeKind.Int => true,
//                        UnionTypeKind.Long => true,
//                        UnionTypeKind.Float => true,
//                        UnionTypeKind.Double => true,
//                        _ => false,
//                    };
//                }

//                case UnionTypeKind.UShort:
//                {
//                    return dest switch {
//                        UnionTypeKind.UShort => true,
//                        UnionTypeKind.Int => true,
//                        UnionTypeKind.UInt => true,
//                        UnionTypeKind.Long => true,
//                        UnionTypeKind.ULong => true,
//                        UnionTypeKind.Float => true,
//                        UnionTypeKind.Double => true,
//                        _ => false,
//                    };
//                }
//            }

//            return false;
//        }
//    }
//}
