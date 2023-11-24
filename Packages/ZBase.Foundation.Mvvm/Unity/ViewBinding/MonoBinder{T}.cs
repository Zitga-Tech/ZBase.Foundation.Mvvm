using System;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    public abstract partial class MonoBinder<TTarget> : MonoBinder
        where TTarget : UnityEngine.Object
    {
        [SerializeField]
        private TTarget[] _targets;

        public ReadOnlyMemory<TTarget> Targets => _targets;

        protected sealed override void OnAwake()
        {
            OnAwake(ref _targets);
            
            _targets ??= Array.Empty<TTarget>();

            OnPostAwake();
        }

        protected virtual void OnPostAwake() { }

        protected abstract void OnAwake(ref TTarget[] targets);
    }
}