using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/GameObject Binder")]
    public partial class GameObjectBinder : MonoBinder
    {
        [SerializeField]
        private GameObject[] _targets = new GameObject[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                _targets = new GameObject[1] { this.gameObject };
            }
        }

        [BindingProperty]
        [field: Label("Active Self")]
        [field: HideInInspector]
        private void SetActive(bool value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.SetActive(value);
                }
            }
        }
    }
}
