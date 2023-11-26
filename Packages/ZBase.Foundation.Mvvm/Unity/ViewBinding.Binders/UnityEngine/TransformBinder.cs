using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Transform Binder")]
    public partial class TransformBinder : MonoBinder<Transform>
    {
        protected sealed override void OnAwake([NotNull] ref Transform[] targets)
        {
            if (targets.Length < 1)
            {
                targets = new Transform[1] { this.gameObject.transform };
            }
        }

        [BindingProperty]
        [field: Label("Position")]
        [field: HideInInspector]
        private void SetPosition(in Vector3 value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].position = value;
            }
        }

        [BindingProperty]
        [field: Label("Rotation")]
        [field: HideInInspector]
        private void SetRotation(in Quaternion value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].rotation = value;
            }
        }

        [BindingProperty]
        [field: Label("Euler Angles")]
        [field: HideInInspector]
        private void SetEulerAngles(in Vector3 value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].eulerAngles = value;
            }
        }

        [BindingProperty]
        [field: Label("Local Position")]
        [field: HideInInspector]
        private void SetLocalPosition(in Vector3 value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].localPosition = value;
            }
        }

        [BindingProperty]
        [field: Label("Local Rotation")]
        [field: HideInInspector]
        private void SetLocalRotation(in Quaternion value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].localRotation = value;
            }
        }

        [BindingProperty]
        [field: Label("Local Euler Angles")]
        [field: HideInInspector]
        private void SetLocalEulerAngles(in Vector3 value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].localEulerAngles = value;
            }
        }

        [BindingProperty]
        [field: Label("Local Scale")]
        [field: HideInInspector]
        private void SetLocalScale(in Vector3 value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].localScale = value;
            }
        }
    }
}
