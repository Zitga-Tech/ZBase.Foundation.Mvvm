using System;
using System.Diagnostics.CodeAnalysis;
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
            _targets ??= Array.Empty<TTarget>();

            OnAwake(ref _targets);
            
            _targets ??= Array.Empty<TTarget>();

            if (_targets.Length < 1)
            {
                WarnIfTargetListIsEmpty(this);
            }

            OnPostAwake();
        }

        protected virtual void OnPostAwake() { }

        protected abstract void OnAwake([NotNull] ref TTarget[] targets);

        [HideInCallstack]
        private static void WarnIfTargetListIsEmpty(UnityEngine.Object context)
        {
            Debug.LogWarning("The target list is empty.", context);
        }
    }
}