#if UNITY_UGUI

using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UI;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Selectable Binder")]
    public partial class SelectableBinder : MonoBinder<Selectable>
    {
        protected sealed override void OnAwake([NotNull] ref Selectable[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<Selectable>(out var target))
                {
                    targets = new Selectable[] { target };
                }
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
    }
}

#endif
