using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Slider Binder")]
    public partial class SliderBinder : MonoBinder<Slider>
    {
        protected sealed override void OnAwake([NotNull] ref Slider[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Slider>(out var target))
                {
                    targets = new Slider[] { target };
                }
            }

            foreach (var target in targets)
            {
                target.onValueChanged.AddListener(OnValueChanged);
            }
        }

        [BindingProperty]
        [field: Label("Interactable")]
        [field: HideInInspector]
        private void SetInteractable(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].interactable = value;
            }
        }

        [BindingProperty]
        [field: Label("Value")]
        [field: HideInInspector]
        private void SetValue(float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].SetValueWithoutNotify(value);
            }
        }

        [BindingProperty]
        [field: Label("Min Value")]
        [field: HideInInspector]
        private void SetMinValue(float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].minValue = value;
            }
        }

        [BindingProperty]
        [field: Label("Max Value")]
        [field: HideInInspector]
        private void SetMaxValue(float value)
        {
            var targets = Targets.Span;
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
            var targets = Targets.Span;
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
