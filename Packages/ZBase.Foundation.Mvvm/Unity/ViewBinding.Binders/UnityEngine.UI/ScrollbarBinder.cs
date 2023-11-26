using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Scroll Bar Binder")]
    public partial class ScrollbarBinder : MonoBinder<Scrollbar>
    {
        protected sealed override void OnAwake([NotNull] ref Scrollbar[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Scrollbar>(out var target))
                {
                    targets = new Scrollbar[] { target };
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
        [field: Label("Size")]
        [field: HideInInspector]
        private void SetSize(float value)
        {
            var targets = Targets.Span;
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
            var targets = Targets.Span;
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
