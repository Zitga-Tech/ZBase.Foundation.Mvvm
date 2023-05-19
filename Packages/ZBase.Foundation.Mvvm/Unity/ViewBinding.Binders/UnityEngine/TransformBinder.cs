using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Transform Binder")]
    public partial class TransformBinder : MonoBinder
    {
        [SerializeField]
        private Transform[] _targets = new Transform[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                _targets = new Transform[1] { this.gameObject.transform };
            }
        }

        [Binding]
        [field: Label("Position")]
        [field: HideInInspector]
        private void SetPosition(in Vector3 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.position = value;
                }
            }
        }

        [Binding]
        [field: Label("Rotation")]
        [field: HideInInspector]
        private void SetRotation(in Quaternion value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.rotation = value;
                }
            }
        }

        [Binding]
        [field: Label("Euler Angles")]
        [field: HideInInspector]
        private void SetEulerAngles(in Vector3 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.eulerAngles = value;
                }
            }
        }

        [Binding]
        [field: Label("Local Position")]
        [field: HideInInspector]
        private void SetLocalPosition(in Vector3 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.localPosition = value;
                }
            }
        }

        [Binding]
        [field: Label("Local Rotation")]
        [field: HideInInspector]
        private void SetLocalRotation(in Quaternion value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.localRotation = value;
                }
            }
        }

        [Binding]
        [field: Label("Local Euler Angles")]
        [field: HideInInspector]
        private void SetLocalEulerAngles(in Vector3 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.localEulerAngles = value;
                }
            }
        }

        [Binding]
        [field: Label("Local Scale")]
        [field: HideInInspector]
        private void SetLocalScale(in Vector3 value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.localScale = value;
                }
            }
        }
    }
}
