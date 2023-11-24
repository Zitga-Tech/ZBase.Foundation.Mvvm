using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Toggle Binder")]
    public partial class ToggleBinder : MonoBinder<Toggle>
    {
        protected sealed override void OnAwake(ref Toggle[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Toggle>(out var target))
                {
                    targets = new Toggle[] { target };
                }
            }

            if (targets.Length < 1)
            {
                Logger.WarnIfTargetListIsEmpty(this);
                return;
            }

            foreach (var target in targets)
            {
                target.onValueChanged.AddListener(OnValueChanged);
            }
        }

        [BindingProperty]
        [field: Label("Is On")]
        [field: HideInInspector]
        private void SetIsOn(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].SetIsOnWithoutNotify(value);
            }
        }

        [BindingCommand]
        [field: Label("On Value Changed")]
        [field: HideInInspector]
        partial void OnValueChanged(bool value);
    }
}
