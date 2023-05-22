using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/RectTransform Binder")]
    public partial class RectTransformBinder : MonoBinder
    {
        [SerializeField]
        private RectTransform[] _targets = new RectTransform[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                if (this.gameObject.transform is RectTransform rectTransform)
                {
                    _targets = new RectTransform[] { rectTransform };
                }
            }

            if (_targets.Length < 1)
            {
                Debug.LogWarning($"The target list is empty.", this);
            }
        }

        [BindingProperty]
        [field: Label("Anchor Min")]
        [field: HideInInspector]
        private void SetAnchorMin(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].anchorMin = value;
            }
        }

        [BindingProperty]
        [field: Label("Anchor Max")]
        [field: HideInInspector]
        private void SetAnchorMax(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].anchorMax = value;
            }
        }

        [BindingProperty]
        [field: Label("Offest Min")]
        [field: HideInInspector]
        private void SetOffsetMin(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].offsetMin = value;
            }
        }

        [BindingProperty]
        [field: Label("Offest Max")]
        [field: HideInInspector]
        private void SetOffsetMax(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].offsetMax = value;
            }
        }

        [BindingProperty]
        [field: Label("Anchored Position")]
        [field: HideInInspector]
        private void SetAnchoredPosition(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].anchoredPosition = value;
            }
        }

        [BindingProperty]
        [field: Label("Anchored Position 3D")]
        [field: HideInInspector]
        private void SetAnchoredPosition3D(in Vector3 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].anchoredPosition3D = value;
            }
        }

        [BindingProperty]
        [field: Label("Size Delta")]
        [field: HideInInspector]
        private void SetSizeDelta(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].sizeDelta = value;
            }
        }

        [BindingProperty]
        [field: Label("Pivot")]
        [field: HideInInspector]
        private void SetPivot(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].pivot = value;
            }
        }
    }
}
