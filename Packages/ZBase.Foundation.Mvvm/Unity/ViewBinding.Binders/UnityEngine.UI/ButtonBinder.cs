using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Button Binder")]
    public partial class ButtonBinder : MonoBinder
    {
        [SerializeField]
        private Button[] _targets = new Button[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Button>(out var target))
                {
                    _targets = new Button[] { target };
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
                    target.onClick.AddListener(OnClick);
                }
            }
        }

        [BindingCommand]
        [field: Label("On Click")]
        [field: HideInInspector]
        partial void OnClick();
    }
}
