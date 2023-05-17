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
                Debug.LogWarning($"The toggle list is empty.", this);
            }
        }

        [Binding]
        [field: Label("Is On")]
        [field: HideInInspector]
        private void SetActive(bool isOn)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.isOn = isOn;
                }
            }
        }
    }
}
