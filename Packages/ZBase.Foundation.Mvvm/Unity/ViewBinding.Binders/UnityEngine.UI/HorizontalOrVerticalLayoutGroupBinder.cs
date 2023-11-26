using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Horizontal Or Vertical Layout Group Binder")]
    public partial class HorizontalOrVerticalLayoutGroupBinder : MonoBinder<HorizontalOrVerticalLayoutGroup>
    {
        protected override void OnAwake([NotNull] ref HorizontalOrVerticalLayoutGroup[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<HorizontalOrVerticalLayoutGroup>(out var target))
                {
                    targets = new HorizontalOrVerticalLayoutGroup[] { target };
                }
            }
        }

        [BindingProperty]
        [field: Label("Spacing")]
        [field: HideInInspector]
        private void SetSpacing(float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].spacing = value;
            }
        }

        [BindingProperty]
        [field: Label("Spacing")]
        [field: HideInInspector]
        private void SetChildAlignment(TextAnchor value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].childAlignment = value;
            }
        }

        [BindingProperty]
        [field: Label("Reverse Arrangement")]
        [field: HideInInspector]
        private void SetReverseArrangement(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].reverseArrangement = value;
            }
        }

        [BindingProperty]
        [field: Label("Control Child Width")]
        [field: HideInInspector]
        private void SetControlChildWidth(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].childControlWidth = value;
            }
        }

        [BindingProperty]
        [field: Label("Control Child Height")]
        [field: HideInInspector]
        private void SetControlChildHeight(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].childControlHeight = value;
            }
        }

        [BindingProperty]
        [field: Label("Use Child Scale Width")]
        [field: HideInInspector]
        private void SetUseChildScaleWidth(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].childScaleWidth = value;
            }
        }

        [BindingProperty]
        [field: Label("Use Child Scale Height")]
        [field: HideInInspector]
        private void SetUseChildScaleHeight(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].childScaleHeight = value;
            }
        }

        [BindingProperty]
        [field: Label("Child Force Expand Width")]
        [field: HideInInspector]
        private void SetChildForceExpandWidth(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].childForceExpandWidth = value;
            }
        }

        [BindingProperty]
        [field: Label("Child Force Expand Height")]
        [field: HideInInspector]
        private void SetChildForceExpandHeight(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].childForceExpandHeight = value;
            }
        }
    }
}
