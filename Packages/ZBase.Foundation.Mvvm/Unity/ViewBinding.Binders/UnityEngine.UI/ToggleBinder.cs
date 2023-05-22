using System;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Toggle Binder")]
    public partial class ToggleBinder : MonoBinder
    {
        [SerializeField]
        private Toggle[] _targets = new Toggle[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Toggle>(out var target))
                {
                    _targets = new Toggle[] { target };
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
        [field: Label("Is On")]
        [field: HideInInspector]
        private void SetIsOn(bool value)
        {
            var targets = _targets.AsSpan();
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
