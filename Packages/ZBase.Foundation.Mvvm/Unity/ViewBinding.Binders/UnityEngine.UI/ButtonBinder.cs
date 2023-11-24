using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Button Binder")]
    public partial class ButtonBinder : MonoBinder<Button>
    {
        protected sealed override void OnAwake(ref Button[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Button>(out var target))
                {
                    targets = new Button[] { target };
                }
            }

            if (targets.Length < 1)
            {
                Logger.WarnIfTargetListIsEmpty(this);
                return;
            }

            foreach (var target in targets)
            {
                target.onClick.AddListener(OnClick);
            }
        }

        [BindingCommand]
        [field: Label("On Click")]
        [field: HideInInspector]
        partial void OnClick();
    }
}
