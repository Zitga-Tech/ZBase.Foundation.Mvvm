using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Image Binder")]
    public partial class ImageBinder : MonoBinder<Image>
    {
        protected sealed override void OnAwake(ref Image[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Image>(out var target))
                {
                    targets = new Image[] { target };
                }
            }

            if (targets.Length < 1)
            {
                Logger.WarnIfTargetListIsEmpty(this);
            }
        }

        [BindingProperty]
        [field: Label("Color")]
        [field: HideInInspector]
        private void SetColor(Color value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].color = value;
            }
        }

        [BindingProperty]
        [field: Label("Sprite")]
        [field: HideInInspector]
        private void SetSprite(Sprite value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].sprite = value;
            }
        }

        [BindingProperty]
        [field: Label("Material")]
        [field: HideInInspector]
        private void SetMaterial(Material value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].material = value;
            }
        }

        [BindingProperty]
        [field: Label("Fill Method")]
        [field: HideInInspector]
        private void SetFillMethod(Image.FillMethod value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].fillMethod = value;
            }
        }

        [BindingProperty]
        [field: Label("Fill Origin")]
        [field: HideInInspector]
        private void SetFillOrigin(int value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].fillOrigin = value;
            }
        }

        [BindingProperty]
        [field: Label("Fill Amount")]
        [field: HideInInspector]
        private void SetFillAmount(float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].fillAmount = value;
            }
        }

        [BindingProperty]
        [field: Label("Raycast Target")]
        [field: HideInInspector]
        private void SetRaycastTarget(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].raycastTarget = value;
            }
        }

        [BindingProperty]
        [field: Label("Maskable")]
        [field: HideInInspector]
        private void SetMaskable(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].maskable = value;
            }
        }
    }
}
