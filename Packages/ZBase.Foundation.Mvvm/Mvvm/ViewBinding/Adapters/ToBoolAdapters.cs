using System;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ViewBinding.Adapters
{
    [Serializable]
    [Label("Bool ⇒ Bool", "Default")]
    [Adapter(fromType: typeof(bool), toType: typeof(bool), order: 0)]
    public sealed class BoolToBoolAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out bool result))
            {
                return result;
            }

            return union;
        }
    }

    [Serializable]
    [Label("String ⇒ Bool", "Default")]
    [Adapter(fromType: typeof(string), toType: typeof(bool), order: 0)]
    public sealed class StringToBoolAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out string result))
            {
                if (bool.TryParse(result, out var value))
                {
                    return value;
                }

                return string.IsNullOrEmpty(result) == false;
            }

            return union;
        }
    }

    [Serializable]
    [Label("Object ⇒ Bool", "Default")]
    [Adapter(fromType: typeof(object), toType: typeof(bool), order: 0)]
    public sealed class ObjectToBoolAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out object result))
            {
                return result != null;
            }

            return union;
        }
    }

    [Serializable]
    [Label("Bool ⇒ ! Bool", "Default")]
    [Adapter(fromType: typeof(bool), toType: typeof(bool), order: 1)]
    public sealed class BoolToNotBoolAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out bool result))
            {
                return !result;
            }

            return union;
        }
    }

    [Serializable]
    [Label("String ⇒ ! Bool", "Default")]
    [Adapter(fromType: typeof(string), toType: typeof(bool), order: 1)]
    public sealed class StringToNotBoolAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out string result))
            {
                if (bool.TryParse(result, out var value))
                {
                    return !value;
                }

                return string.IsNullOrEmpty(result) == false;
            }

            return union;
        }
    }

    [Serializable]
    [Label("Object ⇒ ! Bool", "Default")]
    [Adapter(fromType: typeof(object), toType: typeof(bool), order: 1)]
    public sealed class ObjectToNotBoolAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out object result))
            {
                return result == null;
            }

            return union;
        }
    }
}
