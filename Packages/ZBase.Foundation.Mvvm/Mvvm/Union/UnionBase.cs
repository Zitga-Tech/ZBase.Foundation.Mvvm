using System.Runtime.InteropServices;

namespace ZBase.Foundation.Mvvm.Unions
{
    /// <summary>
    /// Represents a base layout for the <see cref="Union"/> type.
    /// <br/>
    /// The first 8 bytes are used to store meta data.
    /// While the rest are used to store the actual data.
    /// </summary>
    /// <seealso cref="UnionData"/>
    [StructLayout(LayoutKind.Explicit, Size = UnionData.SIZE)]
    public readonly struct UnionBase
    {
        public const int META_SIZE = sizeof(ulong);
        public const int META_OFFSET = 0;
        public const int DATA_OFFSET = META_OFFSET + META_SIZE;

        [FieldOffset(META_OFFSET)] public readonly ulong Meta;
        [FieldOffset(DATA_OFFSET)] public readonly UnionData Data;

        public UnionBase(ulong meta, in UnionData data)
        {
            Meta = meta;
            Data = data;
        }
    }
}
