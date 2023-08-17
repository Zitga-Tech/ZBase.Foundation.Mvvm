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
                Logger.WarnIfTargetListIsEmpty(this);
            }
        }

        [BindingProperty]
        [field: Label("Text")]
        [field: HideInInspector]
        private void SetText(string value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].text = value;
            }
        }

        [BindingProperty]
        [field: Label("Color")]
        [field: HideInInspector]
        private void SetColor(in Color value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].color = value;
            }
        }

        [BindingProperty]
        [field: Label("Font Asset")]
        [field: HideInInspector]
        private void SetFontAsset(TMP_FontAsset value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].font = value;
            }
        }

        [BindingProperty]
        [field: Label("Font Size")]
        [field: HideInInspector]
        private void SetFontSize(float value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].fontSize = value;
            }
        }

        [BindingProperty]
        [field: Label("Auto Sizing")]
        [field: HideInInspector]
        private void SetAutoSizing(bool value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].enableAutoSizing = value;
            }
        }

        [BindingProperty]
        [field: Label("Font Size Min")]
        [field: HideInInspector]
        private void SetFontSizeMin(float value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].fontSizeMin = value;
            }
        }

        [BindingProperty]
        [field: Label("Font Size Max")]
        [field: HideInInspector]
        private void SetFontSizeMax(float value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].fontSizeMax = value;
            }
        }
    }
}

#endif
