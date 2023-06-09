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
            else
            {
                foreach (var target in _targets)
                {
                    target.onValueChanged.AddListener(OnValueChanged);
                }
            }
        }

        [BindingProperty]
        [field: Label("Value")]
        [field: HideInInspector]
        private void SetValue(float value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].SetValueWithoutNotify(value);
            }
        }

        [BindingProperty]
        [field: Label("Size")]
        [field: HideInInspector]
        private void SetSize(float value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].size = value;
            }
        }

        [BindingProperty]
        [field: Label("Number Of Steps")]
        [field: HideInInspector]
        private void SetNumberOfSteps(int value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].numberOfSteps = value;
            }
        }

        [BindingCommand]
        [field: Label("On Value Changed")]
        [field: HideInInspector]
        partial void OnValueChanged(float value);
    }
}
