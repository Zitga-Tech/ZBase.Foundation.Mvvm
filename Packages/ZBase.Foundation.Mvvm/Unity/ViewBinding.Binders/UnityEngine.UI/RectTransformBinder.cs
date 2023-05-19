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

        [Binding]
        [field: Label("Anchor Min")]
        [field: HideInInspector]
        private void SetAnchorMin(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.anchorMin = value;
                }
            }
        }

        [Binding]
        [field: Label("Anchor Max")]
        [field: HideInInspector]
        private void SetAnchorMax(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.anchorMax = value;
                }
            }
        }

        [Binding]
        [field: Label("Offest Min")]
        [field: HideInInspector]
        private void SetOffsetMin(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.offsetMin = value;
                }
            }
        }

        [Binding]
        [field: Label("Offest Max")]
        [field: HideInInspector]
        private void SetOffsetMax(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.offsetMax = value;
                }
            }
        }

        [Binding]
        [field: Label("Anchored Position")]
        [field: HideInInspector]
        private void SetAnchoredPosition(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.anchoredPosition = value;
                }
            }
        }

        [Binding]
        [field: Label("Anchored Position 3D")]
        [field: HideInInspector]
        private void SetAnchoredPosition3D(in Vector3 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.anchoredPosition3D = value;
                }
            }
        }

        [Binding]
        [field: Label("Size Delta")]
        [field: HideInInspector]
        private void SetSizeDelta(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.sizeDelta = value;
                }
            }
        }

        [Binding]
        [field: Label("Pivot")]
        [field: HideInInspector]
        private void SetPivot(in Vector2 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.pivot = value;
                }
            }
        }
    }
}
