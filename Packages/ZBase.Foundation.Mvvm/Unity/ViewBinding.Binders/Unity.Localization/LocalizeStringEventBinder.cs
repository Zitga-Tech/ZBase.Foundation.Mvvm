#if UNITY_LOCALIZATION

#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable CA1815 // Override equals and operator equals on value types

using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/LocalizeStringEvent Binder")]
    public partial class LocalizeStringEventBinder : MonoBinder<LocalizeStringEvent>
    {
        protected sealed override void OnAwake([NotNull] ref LocalizeStringEvent[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<LocalizeStringEvent>(out var target))
                {
                    targets = new LocalizeStringEvent[] { target };
                }
            }
        }

        [BindingProperty]
        [field: Label("Table")]
        [field: HideInInspector]
        private void SetTable(string value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].SetTable(value);
            }
        }

        [BindingProperty]
        [field: Label("Entry")]
        [field: HideInInspector]
        private void SetEntry(string value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].SetEntry(value);
            }
        }

        [BindingProperty]
        [field: Label("Localized IVariable")]
        [field: HideInInspector]
        private void SetIVariable(LocalizedIVariable v)
        {
            var (name, value) = v;

            if (value == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = "0";
            }

            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                var stringRef = target.StringReference;

                if (stringRef.ContainsKey(name) == false)
                {
                    stringRef.Add(name, value);
                }

                target.RefreshString();
            }
        }

        [BindingProperty]
        [field: Label("Localized IVariable Array")]
        [field: HideInInspector]
        private void SetIVariables(LocalizedIVariable[] v)
        {
            var vars = v.AsSpan();

            if (vars.Length < 1)
            {
                return;
            }

            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                var stringRef = target.StringReference;

                for (var k = 0; k < vars.Length; k++)
                {
                    var (name, value) = vars[k];

                    if (value == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(name))
                    {
                        name = "0";
                    }

                    if (stringRef.ContainsKey(name) == false)
                    {
                        stringRef.Add(name, value);
                    }
                }

                target.RefreshString();
            }
        }

        [BindingProperty]
        [field: Label("Localized String Variable")]
        [field: HideInInspector]
        private void SetVariable(LocalizedStringVariable v)
        {
            var (name, value) = v;

            if (value == null)
            {
                return;
            }

            if (string.IsNullOrEmpty(name))
            {
                name = "0";
            }

            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                var stringRef = target.StringReference;

                if (stringRef.TryGetValue(name, out var variable) == false)
                {
                    variable = new StringVariable();
                    stringRef.Add(name, variable);
                }

                if (variable is StringVariable stringVariable)
                {
                    stringVariable.Value = value;
                    target.RefreshString();
                }
            }
        }

        [BindingProperty]
        [field: Label("Localized String Variable Array")]
        [field: HideInInspector]
        private void SetVariables(LocalizedStringVariable[] v)
        {
            var vars = v.AsSpan();

            if (vars.Length < 1)
            {
                return;
            }

            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                var stringRef = target.StringReference;
                var hasChanged = false;

                for (var k = 0; k < vars.Length; k++)
                {
                    var (name, value) = vars[k];

                    if (value == null)
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(name))
                    {
                        name = "0";
                    }

                    if (stringRef.TryGetValue(name, out var variable) == false)
                    {
                        variable = new StringVariable();
                        stringRef.Add(name, variable);
                    }

                    if (variable is StringVariable stringVariable)
                    {
                        stringVariable.Value = value;
                        hasChanged = true;
                    }
                }

                if (hasChanged)
                {
                    target.RefreshString();
                }
            }
        }
    }

    public readonly struct LocalizedStringVariable
    {
        private readonly string _name;
        private readonly string _value;

        public LocalizedStringVariable(string value, string name = "0")
        {
            _name = name;
            _value = value;
        }

        public void Deconstruct(out string name, out string value)
        {
            name = _name;
            value = _value;
        }
    }

    public readonly struct LocalizedIVariable
    {
        private readonly string _name;
        private readonly IVariable _value;

        public LocalizedIVariable(IVariable value, string name = "0")
        {
            _name = name;
            _value = value;
        }

        public void Deconstruct(out string name, out IVariable value)
        {
            name = _name;
            value = _value;
        }
    }
}

#endif
