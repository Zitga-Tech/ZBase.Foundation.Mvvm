#if UNION_SIZE_16_BYTES || UNION_SIZE_24_BYTES || UNION_SIZE_32_BYTES || UNION_SIZE_40_BYTES || UNION_SIZE_48_BYTES || UNION_SIZE_56_BYTES || UNION_SIZE_64_BYTES || UNION_SIZE_72_BYTES || UNION_SIZE_80_BYTES || UNION_SIZE_88_BYTES || UNION_SIZE_96_BYTES || UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L2__
#endif

#if UNION_SIZE_24_BYTES || UNION_SIZE_32_BYTES || UNION_SIZE_40_BYTES || UNION_SIZE_48_BYTES || UNION_SIZE_56_BYTES || UNION_SIZE_64_BYTES || UNION_SIZE_72_BYTES || UNION_SIZE_80_BYTES || UNION_SIZE_88_BYTES || UNION_SIZE_96_BYTES || UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L3__
#endif

#if UNION_SIZE_32_BYTES || UNION_SIZE_40_BYTES || UNION_SIZE_48_BYTES || UNION_SIZE_56_BYTES || UNION_SIZE_64_BYTES || UNION_SIZE_72_BYTES || UNION_SIZE_80_BYTES || UNION_SIZE_88_BYTES || UNION_SIZE_96_BYTES || UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L4__
#endif

#if UNION_SIZE_40_BYTES || UNION_SIZE_48_BYTES || UNION_SIZE_56_BYTES || UNION_SIZE_64_BYTES || UNION_SIZE_72_BYTES || UNION_SIZE_80_BYTES || UNION_SIZE_88_BYTES || UNION_SIZE_96_BYTES || UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L5__
#endif

#if UNION_SIZE_48_BYTES || UNION_SIZE_56_BYTES || UNION_SIZE_64_BYTES || UNION_SIZE_72_BYTES || UNION_SIZE_80_BYTES || UNION_SIZE_88_BYTES || UNION_SIZE_96_BYTES || UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L6__
#endif

#if UNION_SIZE_56_BYTES || UNION_SIZE_64_BYTES || UNION_SIZE_72_BYTES || UNION_SIZE_80_BYTES || UNION_SIZE_88_BYTES || UNION_SIZE_96_BYTES || UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L7__
#endif

#if UNION_SIZE_64_BYTES || UNION_SIZE_72_BYTES || UNION_SIZE_80_BYTES || UNION_SIZE_88_BYTES || UNION_SIZE_96_BYTES || UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L8__
#endif

#if UNION_SIZE_72_BYTES || UNION_SIZE_80_BYTES || UNION_SIZE_88_BYTES || UNION_SIZE_96_BYTES || UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L9__
#endif

#if UNION_SIZE_80_BYTES || UNION_SIZE_88_BYTES || UNION_SIZE_96_BYTES || UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L10__
#endif

#if UNION_SIZE_88_BYTES || UNION_SIZE_96_BYTES || UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L11__
#endif

#if UNION_SIZE_96_BYTES || UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L12__
#endif

#if UNION_SIZE_104_BYTES || UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L13__
#endif

#if UNION_SIZE_112_BYTES || UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L14__
#endif

#if UNION_SIZE_120_BYTES || UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L15__
#endif

#if UNION_SIZE_128_BYTES
#define __UNION_STORAGE_ENABLE_L16__
#endif

using System.Runtime.InteropServices;

namespace ZBase.Foundation.Unions
{
    /// <summary>
    /// Represents a memory layout that can store the actual data of several types.
    /// </summary>
    /// <remarks>
    /// By default, the native size is 8 bytes.
    /// <br />
    /// To resize, define one of the following symbols:
    /// <list type="bullet">
    /// <item><c>UNION_SIZE_16_BYTES</c></item>
    /// <item><c>UNION_SIZE_24_BYTES</c></item>
    /// <item><c>UNION_SIZE_32_BYTES</c></item>
    /// <item><c>UNION_SIZE_40_BYTES</c></item>
    /// <item><c>UNION_SIZE_48_BYTES</c></item>
    /// <item><c>UNION_SIZE_56_BYTES</c></item>
    /// <item><c>UNION_SIZE_64_BYTES</c></item>
    /// <item><c>UNION_SIZE_72_BYTES</c></item>
    /// <item><c>UNION_SIZE_80_BYTES</c></item>
    /// <item><c>UNION_SIZE_88_BYTES</c></item>
    /// <item><c>UNION_SIZE_96_BYTES</c></item>
    /// <item><c>UNION_SIZE_104_BYTES</c></item>
    /// <item><c>UNION_SIZE_112_BYTES</c></item>
    /// <item><c>UNION_SIZE_120_BYTES</c></item>
    /// <item><c>UNION_SIZE_128_BYTES</c></item>
    /// </list>
    /// </remarks>
    [StructLayout(LayoutKind.Sequential)]
    public readonly struct UnionData
    {
        private readonly ulong _l1;

#if __UNION_STORAGE_ENABLE_L3__
        private readonly ulong _l2;
#endif

#if __UNION_STORAGE_ENABLE_L3__
        private readonly ulong _l3;
#endif

#if __UNION_STORAGE_ENABLE_L4__
        private readonly ulong _l4;
#endif

#if __UNION_STORAGE_ENABLE_L5__
        private readonly ulong _l5;
#endif

#if __UNION_STORAGE_ENABLE_L6__
        private readonly ulong _l6;
#endif

#if __UNION_STORAGE_ENABLE_L7__
        private readonly ulong _l7;
#endif

#if __UNION_STORAGE_ENABLE_L8__
        private readonly ulong _l8;
#endif

#if __UNION_STORAGE_ENABLE_L9__
        private readonly ulong _l9;
#endif

#if __UNION_STORAGE_ENABLE_L10__
        private readonly ulong _l10;
#endif

#if __UNION_STORAGE_ENABLE_L11__
        private readonly ulong _l11;
#endif

#if __UNION_STORAGE_ENABLE_L12__
        private readonly ulong _l12;
#endif

#if __UNION_STORAGE_ENABLE_L13__
        private readonly ulong _l13;
#endif

#if __UNION_STORAGE_ENABLE_L14__
        private readonly ulong _l14;
#endif

#if __UNION_STORAGE_ENABLE_L15__
        private readonly ulong _l15;
#endif

#if __UNION_STORAGE_ENABLE_L16__
        private readonly ulong _l16;
#endif
    }
}
