#if UNITY_TEXT_MESH_PRO

using System;
using TMPro;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/TMP_Text Binder")]
    public partial class TMP_TextBinder : MonoBinder
    {
        [SerializeField]
        private TMP_Text[] _targets = new TMP_Text[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<TMP_Text>(out var target))
                {
                    _targets = new TMP_Text[] { target };
                }
            }

            if (_targets.Length < 1)
            {
                Debug.LogWarning($"The target list is empty.", this);
            }
        }

        [Binding("Text")]
        [field: HideInInspector]
        private void SetText(string value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.text = value;
                }
            }
        }
    }
}

#endif
