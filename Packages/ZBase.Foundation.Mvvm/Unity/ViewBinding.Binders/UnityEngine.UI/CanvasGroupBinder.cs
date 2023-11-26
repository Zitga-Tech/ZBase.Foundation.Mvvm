using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Canvas Group Binder")]
    public partial class CanvasGroupBinder : MonoBinder<CanvasGroup>
    {
        protected override void OnAwake([NotNull] ref CanvasGroup[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<CanvasGroup>(out var target))
                {
                    targets = new CanvasGroup[] { target };
                }
            }
        }

        [BindingProperty]
        [field: Label("Alpha")]
        [field: HideInInspector]
        private void SetAlpha( float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].alpha = value;
            }
        }

        [BindingProperty]
        [field: Label("Block Raycasts")]
        [field: HideInInspector]
        private void SetBlockRaycast(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].blocksRaycasts = value;
            }
        }

        [BindingProperty]
        [field: Label("Interactable")]
        [field: HideInInspector]
        private void SetInteractable(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].interactable = value;
            }
        }
    }
}
