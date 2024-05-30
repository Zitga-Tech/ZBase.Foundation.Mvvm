using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    public abstract partial class MonoBinder<TTarget> : MonoBinder, IBinder
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
                WarnTargetListIsEmpty(this);
            }

            OnPostAwake();
        }

        protected virtual void OnPostAwake() { }

        protected abstract void OnAwake([NotNull] ref TTarget[] targets);

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void WarnTargetListIsEmpty(UnityEngine.Object context)
        {
            UnityEngine.Debug.LogWarning("The target list is empty.", context);
        }
    }
}