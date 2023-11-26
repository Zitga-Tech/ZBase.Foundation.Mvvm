using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Scroll Rect Binder")]
    public partial class ScrollRectBinder : MonoBinder<ScrollRect>
    {
        protected sealed override void OnAwake([NotNull] ref ScrollRect[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<ScrollRect>(out var target))
                {
                    targets = new ScrollRect[] { target };
                }
            }

            foreach (var target in targets)
            {
                target.onValueChanged.AddListener(OnValueChanged);
            }
        }

        [BindingProperty]
        [field: Label("Vertical Normalized Position")]
        [field: HideInInspector]
        private void SetVerticalNormalizedPosition(float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].verticalNormalizedPosition = value;
            }
        }

        [BindingProperty]
        [field: Label("Horizontal Normalized Position")]
        [field: HideInInspector]
        private void SetHorizontalNormalizedPosition(float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].horizontalNormalizedPosition = value;
            }
        }

        [BindingCommand]
        [field: Label("On Value Changed")]
        [field: HideInInspector]
        partial void OnValueChanged(Vector2 value);
    }
}
