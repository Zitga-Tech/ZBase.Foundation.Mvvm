using System;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Slider Binder")]
    public partial class SliderBinder : MonoBinder
    {
        [SerializeField]
        private Slider[] _targets = new Slider[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Slider>(out var target))
                {
                    _targets = new Slider[] { target };
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
        [field: Label("Min Value")]
        [field: HideInInspector]
        private void SetMinValue(int value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].minValue = value;
            }
        }

        [BindingProperty]
        [field: Label("Max Value")]
        [field: HideInInspector]
        private void SetMaxValue(int value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].maxValue = value;
            }
        }

        [BindingProperty]
        [field: Label("Whole Numbers")]
        [field: HideInInspector]
        private void SetWholeNumbers(bool value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].wholeNumbers = value;
            }
        }

        [BindingCommand]
        [field: Label("On Value Changed")]
        [field: HideInInspector]
        partial void OnValueChanged(float value);
    }
}
