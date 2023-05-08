﻿using System;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ViewBinding.Adapters
{
    [Serializable]
    [Label("String ⇒ String", "Default")]
    [Adapter(fromType: typeof(string), toType: typeof(string), order: 0)]
    public sealed class StringToStringAdapter : IAdapter
    {
        public Union Convert(in Union union)
        {
            if (union.TryGetValue(out string result))
            {
                return result;
            }

            return union;
        }
    }
}
