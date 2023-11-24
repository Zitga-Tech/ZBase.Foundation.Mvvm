using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Canvas Binder")]
    public partial class CanvasBinder : MonoBinder<Canvas>
    {
        protected sealed override void OnAwake(ref Canvas[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Canvas>(out var target))
                {
                    targets = new Canvas[] { target };
                }
            }

            if (targets.Length < 1)
            {
                Logger.WarnIfTargetListIsEmpty(this);
            }
        }

        [BindingProperty]
        [field: Label("Camera")]
        [field: HideInInspector]
        private void SetCamera(Camera value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].worldCamera = value;
            }
        }

        [BindingProperty]
        [field: Label("Override Sorting")]
        [field: HideInInspector]
        private void SetOverrideSorting(bool value)
        {
            var targets = Targets.Span;
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
            var targets = Targets.Span;
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
            var targets = Targets.Span;
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
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].sortingOrder = value;
            }
        }
    }
}
