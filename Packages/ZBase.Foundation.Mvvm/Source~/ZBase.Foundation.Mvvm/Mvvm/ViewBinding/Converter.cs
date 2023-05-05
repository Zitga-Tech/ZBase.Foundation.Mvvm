using System;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    [Serializable]
    public sealed class Converter
    {
#if UNITY_5_3_OR_NEWER
        [field: UnityEngine.SerializeReference]
#endif
        public IAdapter Adapter { get; set; }

        public Union Convert(in Union value)
            => Adapter?.Convert(value) ?? value;
    }
}
