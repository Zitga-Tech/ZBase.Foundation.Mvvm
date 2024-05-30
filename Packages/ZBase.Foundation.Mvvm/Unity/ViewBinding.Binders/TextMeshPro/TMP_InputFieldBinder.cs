#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

#pragma warning disable CA1707 // Identifiers should not contain underscores
#pragma warning disable CA1815 // Override equals and operator equals on value types
#pragma warning disable CA1051 // Do not declare visible instance fields

using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace TMPro
{
    public readonly struct TMP_TextSelectionData
    {
        public readonly string Text;
        public readonly int StringPosition;
        public readonly int StringSelectPosition;

        public TMP_TextSelectionData(string text, int stringPosition, int stringSelectPosition)
        {
            Text = text;
            StringPosition = stringPosition;
            StringSelectPosition = stringSelectPosition;
        }
    }
}

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/TMP_InputField Binder")]
    public partial class TMP_InputFieldBinder : MonoBinder<TMP_InputField>, IBinder
    {
        protected sealed override void OnAwake([NotNull] ref TMP_InputField[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<TMP_InputField>(out var target))
                {
                    targets = new TMP_InputField[] { target };
                }
            }

            foreach (var target in targets)
            {
                target.onValueChanged.AddListener(OnValueChanged);
                target.onEndEdit.AddListener(OnEndEdit);
                target.onSubmit.AddListener(OnSubmit);
                target.onSelect.AddListener(OnSelect);
                target.onDeselect.AddListener(OnDeselect);
                target.onTextSelection.AddListener((a, b, c) => OnTextSelection(new(a, b, c)));
                target.onEndTextSelection.AddListener((a, b, c) => OnEndTextSelection(new(a, b, c)));
                target.onTouchScreenKeyboardStatusChanged.AddListener(OnTouchScreenKeyboardStatusChanged);
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

        [BindingProperty]
        [field: Label("Character Limit")]
        [field: HideInInspector]
        private void SetCharacterLimit(int value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].characterLimit = value;
            }
        }

        [BindingProperty]
        [field: Label("Content Type")]
        [field: HideInInspector]
        private void SetContentType(TMP_InputField.ContentType value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].contentType = value;
            }
        }

        [BindingProperty]
        [field: Label("Line Type")]
        [field: HideInInspector]
        private void SetLineType(TMP_InputField.LineType value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].lineType = value;
            }
        }

        [BindingProperty]
        [field: Label("Rich Text")]
        [field: HideInInspector]
        private void SetRichText(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].richText = value;
            }
        }

        [BindingProperty]
        [field: Label("Allow Rich Text Editing")]
        [field: HideInInspector]
        private void SetAllowRichTextEditing(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].isRichTextEditingAllowed = value;
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
                targets[i].SetTextWithoutNotify(value);
            }
        }

        [BindingProperty]
        [field: Label("Text Color")]
        [field: HideInInspector]
        private void SetTextColor(in Color value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].textComponent.color = value;
            }
        }

        [BindingProperty]
        [field: Label("Placeholder Text")]
        [field: HideInInspector]
        private void SetPlaceholderText(string value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                if (targets[i].placeholder is TMP_Text placeholder)
                {
                    placeholder.text = value;
                }
            }
        }

        [BindingProperty]
        [field: Label("Placeholder Color")]
        [field: HideInInspector]
        private void SetPlaceholderColor(in Color value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].placeholder.color = value;
            }
        }

        [BindingProperty]
        [field: Label("Custom Caret Color")]
        [field: HideInInspector]
        private void SetCustomCaretColor(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].customCaretColor = value;
            }
        }

        [BindingProperty]
        [field: Label("Caret Color")]
        [field: HideInInspector]
        private void SetCaretColor(in Color value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].caretColor = value;
            }
        }

        [BindingProperty]
        [field: Label("Selection Color")]
        [field: HideInInspector]
        private void SetSelectionColor(in Color value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].selectionColor = value;
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
                targets[i].fontAsset = value;
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
                targets[i].pointSize = value;
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
                targets[i].textComponent.raycastTarget = value;
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
                targets[i].textComponent.maskable = value;
            }
        }

        [BindingCommand]
        [field: Label("On Value Changed")]
        [field: HideInInspector]
        partial void OnValueChanged(string value);

        [BindingCommand]
        [field: Label("On End Edit")]
        [field: HideInInspector]
        partial void OnEndEdit(string value);

        [BindingCommand]
        [field: Label("On Submit")]
        [field: HideInInspector]
        partial void OnSubmit(string value);
        
        [BindingCommand]
        [field: Label("On Select")]
        [field: HideInInspector]
        partial void OnSelect(string value);

        [BindingCommand]
        [field: Label("On Deselect")]
        [field: HideInInspector]
        partial void OnDeselect(string value);

        [BindingCommand]
        [field: Label("On Text Selection")]
        [field: HideInInspector]
        partial void OnTextSelection(TMP_TextSelectionData value);

        [BindingCommand]
        [field: Label("On End Text Selection")]
        [field: HideInInspector]
        partial void OnEndTextSelection(TMP_TextSelectionData value);

        [BindingCommand]
        [field: Label("On Touch Screen Keyboard Status Changed")]
        [field: HideInInspector]
        partial void OnTouchScreenKeyboardStatusChanged(TouchScreenKeyboard.Status value);
    }
}

#endif
