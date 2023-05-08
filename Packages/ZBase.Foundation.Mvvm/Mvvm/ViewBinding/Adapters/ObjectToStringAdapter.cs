using System;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ViewBinding.Adapters
{
    [Serializable]
    [Label("Object ⇒ String", "Default")]
    [Adapter(fromType: typeof(object), toType: typeof(string), order: 0)]
    public sealed class ObjectToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out object result))
            {
                return result.ToString();
            }

            return union;
        }
    }
}
