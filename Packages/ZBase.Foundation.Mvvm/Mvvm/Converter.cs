using System;
using System.Runtime.CompilerServices;

namespace ZBase.Foundation.Mvvm
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

        public virtual ValueUnion Convert(in ValueUnion value)
            => Adapter?.Convert(value) ?? value;
    }
}
