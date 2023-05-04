using System;
using System.Runtime.CompilerServices;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.Conversion
{
    [Serializable]
    public class Converter
    {
        /// <summary>
        /// Label for the converter
        /// </summary>
#if UNITY_5_3_OR_NEWER
        [field: UnityEngine.SerializeField]
#endif
        public string Label { get; set; }

#if UNITY_5_3_OR_NEWER
        [field: UnityEngine.SerializeReference]
#endif
        private IAdapter _adapter;

        public IAdapter Adapter
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _adapter;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => _adapter = value ?? throw new ArgumentNullException(nameof(value));
        }

        public virtual Union Convert(in Union value)
            => Adapter?.Convert(value) ?? value;
    }
}
