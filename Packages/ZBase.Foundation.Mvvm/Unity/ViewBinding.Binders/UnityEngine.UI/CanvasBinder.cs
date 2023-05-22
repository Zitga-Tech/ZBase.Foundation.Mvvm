using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Canvas Binder")]
    public partial class CanvasBinder : MonoBinder
    {
        [SerializeField]
        private Canvas[] _targets = new Canvas[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Canvas>(out var target))
                {
                    _targets = new Canvas[] { target };
                }
            }

            if (_targets.Length < 1)
            {
                Debug.LogWarning($"The target list is empty.", this);
            }
        }

        [BindingProperty]
        [field: Label("Override Sorting")]
        [field: HideInInspector]
        private void SetOverrideSorting(bool value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].overrideSorting = value;
            }
        }

        [BindingProperty]
        [field: Label("Sorting Layer ID")]
        [field: HideInInspector]
        private void SetSortingLayerId(int value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].sortingLayerID = value;
            }
        }

        [BindingProperty]
        [field: Label("Sorting Layer Name")]
        [field: HideInInspector]
        private void SetSortingLayerName(string value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].sortingLayerName = value;
            }
        }

        [BindingProperty]
        [field: Label("Order In Layer")]
        [field: HideInInspector]
        private void SetOrderInLayer(int value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].sortingOrder = value;
            }
        }
    }
}
