﻿using System;

namespace ZBase.Foundation.Mvvm
{
    [Serializable]
    public sealed class BindingField
    {
        /// <summary>
        /// Label for the binding field
        /// </summary>
#if UNITY_5_3_OR_NEWER
        [field: UnityEngine.SerializeField]
#endif
        public string Label { get; set; }

        /// <summary>
        /// The name of an observable property
        /// whose container class is an <see cref="IObservableObject"/>.
        /// </summary>
#if UNITY_5_3_OR_NEWER
        [field: UnityEngine.SerializeField]
#endif
        public string ObservablePropertyName { get; set; }
    }
}