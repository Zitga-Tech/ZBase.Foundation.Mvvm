using System;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Scrollbar Binder")]
    public partial class ScrollbarBinder : MonoBinder
    {
        [SerializeField]
        private Scrollbar[] _targets = new Scrollbar[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Scrollbar>(out var target))
                {
                    _targets = new Scrollbar[] { target };
                }
            }

            if (_targets.Length < 1)
            {
                Debug.LogWarning($"The target list is empty.", this);
            }
        }

        [Binding]
        [field: Label("Value")]
        [field: HideInInspector]
        private void SetValue(float value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.value = value;
                }
            }
        }

        [Binding]
        [field: Label("Size")]
        [field: HideInInspector]
        private void SetSize(float value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.size = value;
                }
            }
        }

        [Binding]
        [field: Label("Number Of Steps")]
        [field: HideInInspector]
        private void SetNumberOfSteps(int value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.numberOfSteps = value;
                }
            }
        }
    }
}
