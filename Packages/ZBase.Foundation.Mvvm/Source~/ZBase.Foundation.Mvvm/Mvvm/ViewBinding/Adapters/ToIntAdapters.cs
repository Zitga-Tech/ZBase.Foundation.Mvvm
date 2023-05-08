using System;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ViewBinding.Adapters
{
    [Serializable]
    [Label("Bool ⇒ Int", "Default")]
    [Adapter(fromType: typeof(bool), toType: typeof(int), order: 0)]
    public sealed class BoolToIntAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out bool result))
            {
                return result ? 1 : 0;
            }

            return union;
        }
    }

    [Serializable]
    [Label("String ⇒ Int", "Default")]
    [Adapter(fromType: typeof(string), toType: typeof(int), order: 0)]
    public sealed class StringToIntAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out string result)
                && int.TryParse(result, out var value)
            )
            {
                return value;
            }

            return union;
        }
    }

    [Serializable]
    [Label("Object ⇒ Int", "Default")]
    [Adapter(fromType: typeof(object), toType: typeof(int), order: 0)]
    public sealed class ObjectToIntAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out object result)
                && result is int value
            )
            {
                return value;
            }

            return union;
        }
    }
}
