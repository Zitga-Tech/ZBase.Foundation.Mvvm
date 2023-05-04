using System;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    [Serializable]
    public sealed class BindingField
    {
        /// <summary>
        /// The property whose container class is an <see cref="ZBase.Foundation.Mvvm.ComponentModel.IObservableObject"/>.
        /// </summary>
        /// <remarks>
        /// This property must be applicable for
        /// either <see cref="ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanging"/>
        /// or <see cref="ZBase.Foundation.Mvvm.ComponentModel.INotifyPropertyChanged"/>
        /// or both.
        /// </remarks>
#if UNITY_5_3_OR_NEWER
        [field: UnityEngine.SerializeField]
#endif
        public string PropertyName { get; set; }
    }
}
