#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

#pragma warning disable CA1707 // Identifiers should not contain underscores

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using TMPro;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/TMP_Dropdown Binder")]
    public partial class TMP_DropdownBinder : MonoBinder<TMP_Dropdown>, IBinder
    {
        protected sealed override void OnAwake([NotNull] ref TMP_Dropdown[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<TMP_Dropdown>(out var target))
                {
                    targets = new TMP_Dropdown[] { target };
                }
            }

            foreach (var target in targets)
            {
                target.onValueChanged.AddListener(OnValueChanged);
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
        [field: Label("Caption Text")]
        [field: HideInInspector]
        private void SetCaptionText(string value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target.captionText == false)
                {
                    continue;
                }

                target.captionText.text = value;
            }
        }

        [BindingProperty]
        [field: Label("Value")]
        [field: HideInInspector]
        private void SetValue(int value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].SetValueWithoutNotify(value);
            }
        }

        [BindingProperty]
        [field: Label("Set Options")]
        [field: HideInInspector]
        private void SetOptionDataList(List<TMP_Dropdown.OptionData> value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                target.options.Clear();
                target.AddOptions(value);
            }
        }

        [BindingProperty]
        [field: Label("Set Options")]
        [field: HideInInspector]
        private void SetOptionStringList(List<string> value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                target.options.Clear();
                target.AddOptions(value);
            }
        }

        [BindingProperty]
        [field: Label("Set Options")]
        [field: HideInInspector]
        private void SetOptionSpriteList(List<Sprite> value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                target.options.Clear();
                target.AddOptions(value);
            }
        }

        [BindingProperty]
        [field: Label("Add Options")]
        [field: HideInInspector]
        private void AddOptionDataList(List<TMP_Dropdown.OptionData> value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].AddOptions(value);
            }
        }

        [BindingProperty]
        [field: Label("Add Options")]
        [field: HideInInspector]
        private void AddOptionStringList(List<string> value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].AddOptions(value);
            }
        }

        [BindingProperty]
        [field: Label("Add Options")]
        [field: HideInInspector]
        private void AddOptionSpriteList(List<Sprite> value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].AddOptions(value);
            }
        }

        [BindingCommand]
        [field: Label("On Value Changed")]
        [field: HideInInspector]
        partial void OnValueChanged(int value);
    }
}

#endif
