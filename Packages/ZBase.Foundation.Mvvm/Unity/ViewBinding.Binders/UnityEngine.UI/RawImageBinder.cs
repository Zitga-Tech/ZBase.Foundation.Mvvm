#if UNITY_UGUI

using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Raw Image Binder")]
    public partial class RawImageBinder : MonoBinder<RawImage>
    {
        protected sealed override void OnAwake([NotNull] ref RawImage[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<RawImage>(out var target))
                {
                    targets = new RawImage[] { target };
                }
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
        [field: Label("Texture")]
        [field: HideInInspector]
        private void SetTexture(Texture value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].texture = value;
            }
        }

        [BindingProperty]
        [field: Label("Texture (Native Size)")]
        [field: HideInInspector]
        private void SetTextureNativeSize(Texture value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                target.texture = value;
                target.SetNativeSize();
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

        [BindingProperty]
        [field: Label("UV Rect")]
        [field: HideInInspector]
        private void SetUVRect(Rect value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].uvRect = value;
            }
        }
    }
}

#endif
