using System;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ViewBinding.Adapters
{
    [Serializable]
    [Label("Bool ⇒ Float", "Default")]
    [Adapter(fromType: typeof(bool), toType: typeof(float), order: 0)]
    public sealed class BoolToFloatAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out bool result))
            {
                return result ? 1f : 0f;
            }

            return union;
        }
    }

    [Serializable]
    [Label("String ⇒ Float", "Default")]
    [Adapter(fromType: typeof(string), toType: typeof(float), order: 0)]
    public sealed class StringToFloatAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out string result)
                && float.TryParse(result, out var value)
            )
            {
                return value;
            }

            return union;
        }
    }

    [Serializable]
    [Label("Object ⇒ Float", "Default")]
    [Adapter(fromType: typeof(object), toType: typeof(float), order: 0)]
    public sealed class ObjectToFloatAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out object result)
                && result is float value
            )
            {
                return value;
            }

            return union;
        }
    }
}
