using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Graphic Raycaster Binder")]
    public partial class GraphicRaycasterBinder : MonoBinder<GraphicRaycaster>
    {
        protected sealed override void OnAwake(ref GraphicRaycaster[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<GraphicRaycaster>(out var target))
                {
                    targets = new GraphicRaycaster[] { target };
                }
            }

            if (targets.Length < 1)
            {
                Logger.WarnIfTargetListIsEmpty(this);
            }
        }

        [BindingProperty]
        [field: Label("Ignore Reversed Graphics")]
        [field: HideInInspector]
        private void SetIgnoreReversedGraphics(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].ignoreReversedGraphics = value;
            }
        }

        [BindingProperty]
        [field: Label("Blocking Objects")]
        [field: HideInInspector]
        private void SetBlockingObjects(GraphicRaycaster.BlockingObjects value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].blockingObjects = value;
            }
        }

        [BindingProperty]
        [field: Label("Blocking Mask")]
        [field: HideInInspector]
        private void SetBlockingMask(LayerMask value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].blockingMask = value;
            }
        }
    }
}
