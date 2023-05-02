using System.Runtime.InteropServices;

namespace ZBase.Foundation.Unions
{
    public static class UnionAPI
    {
        static UnionAPI()
        {
#if !UNITY_5_3_OR_NEWER
            Init();
#endif
        }

#if UNITY_5_3_OR_NEWER
        [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
        private static void Init()
        {
        }

        public static bool TryGetValue<T>(in Union union, out T result)
        {
            result = default;
            return false;
        }
    }

    public interface IUnionConverter<T>
    {
        public bool TryGetValue(in Union union, out T result)
        {
            result = default;
            return false;
        }

        public bool TrySetValue(in Union union, ref T result)
        {
            return false;
        }
    }

    public enum MyEnum { A, B, C }

    [StructLayout(LayoutKind.Explicit)]
    public readonly partial struct UnionMyEnum : IUnion<MyEnum>
    {
        public static readonly UnionTypeId MyEnumTypeId = UnionTypeId.Of<MyEnum>();

        [FieldOffset(UnionBase.META_OFFSET)] public readonly Union Union;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly MyEnum Value;

        public UnionMyEnum(MyEnum value)
        {
            Union = new Union(UnionTypeKind.UserDefined, MyEnumTypeId);
            Value = value;
        }

        public UnionMyEnum(in Union union) : this()
        {
            Union = union;
        }

        public static implicit operator UnionMyEnum(MyEnum value)
            => new UnionMyEnum(value);

        public static implicit operator Union(in UnionMyEnum value)
            => value.Union;

        public sealed class Converter : IUnionConverter<MyEnum>
        {
            public static readonly Converter Default = new Converter();

            private Converter() { }

            public bool TryGetValue(in Union union, out MyEnum result)
            {
                if (union.TypeId == MyEnumTypeId)
                {
                    var unionMyEnum = new UnionMyEnum(union);
                    result = unionMyEnum.Value;
                    return true;
                }

                result = default;
                return false;
            }

            public bool TrySetValue(in Union union, ref MyEnum result)
            {
                if (union.TypeId == MyEnumTypeId)
                {
                    var unionMyEnum = new UnionMyEnum(union);
                    result = unionMyEnum.Value;
                    return true;
                }

                return false;
            }
        }
    }
}
