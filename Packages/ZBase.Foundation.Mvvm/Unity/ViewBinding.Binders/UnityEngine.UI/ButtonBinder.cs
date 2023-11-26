using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Button Binder")]
    public partial class ButtonBinder : MonoBinder<Button>
    {
        protected sealed override void OnAwake([NotNull] ref Button[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Button>(out var target))
                {
                    targets = new Button[] { target };
                }
            }

            foreach (var target in targets)
            {
                target.onClick.AddListener(OnClick);
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

        [BindingCommand]
        [field: Label("On Click")]
        [field: HideInInspector]
        partial void OnClick();
    }
}
