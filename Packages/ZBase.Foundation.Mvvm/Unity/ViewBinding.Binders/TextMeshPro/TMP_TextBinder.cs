#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

#pragma warning disable CA1707 // Identifiers should not contain underscores

using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/TMP_Text Binder")]
    public partial class TMP_TextBinder : MonoBinder<TMP_Text>, IBinder
    {
        protected sealed override void OnAwake([NotNull] ref TMP_Text[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<TMP_Text>(out var target))
                {
                    targets = new TMP_Text[] { target };
                }
            }
        }

        [BindingProperty]
        [field: Label("Text")]
        [field: HideInInspector]
        private void SetText(string value)
        {
            var targets = Targets.Span;
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
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].color = value;
            }
        }

        [BindingProperty]
        [field: Label("Max Visible Characters")]
        [field: HideInInspector]
        private void SetMaxVisibleCharacters(int value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].maxVisibleCharacters = value;
            }
        }

        [BindingProperty]
        [field: Label("Max Visible Words")]
        [field: HideInInspector]
        private void SetMaxVisibleWords(int value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].maxVisibleWords = value;
            }
        }

        [BindingProperty]
        [field: Label("Max Visible Lines")]
        [field: HideInInspector]
        private void SetMaxVisibleLines(int value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].maxVisibleLines = value;
            }
        }

        [BindingProperty]
        [field: Label("Font Asset")]
        [field: HideInInspector]
        private void SetFontAsset(TMP_FontAsset value)
        {
            var targets = Targets.Span;
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
            var targets = Targets.Span;
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
            var targets = Targets.Span;
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
            var targets = Targets.Span;
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
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].fontSizeMax = value;
            }
        }

        [BindingProperty]
        [field: Label("Raycast Target")]
        [field: HideInInspector]
        private void SetRaycastTarget(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].raycastTarget = value;
            }
        }

        [BindingProperty]
        [field: Label("Maskable")]
        [field: HideInInspector]
        private void SetMaskable(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].maskable = value;
            }
        }

        [BindingProperty]
        [field: Label("Word Wrapping")]
        [field: HideInInspector]
        private void SetWordWrapping(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].enableWordWrapping = value;
            }
        }

        [BindingProperty]
        [field: Label("Overflow Mode")]
        [field: HideInInspector]
        private void SetOverflowMode(TextOverflowModes value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].overflowMode = value;
            }
        }
    }
}

#endif
