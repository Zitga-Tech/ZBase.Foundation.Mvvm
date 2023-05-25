using System.Runtime.InteropServices;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.Collections
{
    public readonly struct RefTypeHandle
    {
        public readonly GCHandle Value;

        public RefTypeHandle(GCHandle value)
        {
            Value = value;
        }
    }

    public readonly partial struct RefTypeHandleUnion : IUnion<RefTypeHandle> { }
}