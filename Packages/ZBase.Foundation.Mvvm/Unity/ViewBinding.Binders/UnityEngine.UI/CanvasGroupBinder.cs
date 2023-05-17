using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/CanvasGroup Binder")]
    public partial class CanvasGroupBinder : MonoBinder
    {
        [SerializeField]
        private CanvasGroup[] _targets = new CanvasGroup[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<CanvasGroup>(out var target))
                {
                    _targets = new CanvasGroup[] { target };
                }
            }

            if (_targets.Length < 1)
            {
                Debug.LogWarning($"The CanvasGroup list is empty.", this);
            }
        }

        [Binding]
        [field: Label("Alpha")]
        [field: HideInInspector]
        private void SetAlpha( float alpha)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.alpha = alpha;
                }
            }
        }

        [Binding]
        [field: Label("Block Raycasts")]
        [field: HideInInspector]
        private void SetBlockRaycast(bool blockRaycast)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.blocksRaycasts = blockRaycast;
                }
            }
        }

        [Binding]
        [field: Label("Interactable")]
        [field: HideInInspector]
        private void SetInteractable(bool interactable)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.interactable = interactable;
                }
            }
        }
    }
}
