using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Behaviour Binder")]
    public partial class BehaviourBinder : MonoBinder<Behaviour>
    {
        [SerializeField]
        private bool _includeMonoBinders = false;

        protected sealed override void OnAwake([NotNull] ref Behaviour[] targets)
        {
            var container = new List<Behaviour>();
            container.AddRange(targets);

            if (container.Count < 1)
            {
                this.gameObject.GetComponents(container);
            }

            if (_includeMonoBinders == false)
            {
                for (var i = container.Count - 1; i >= 0; i--)
                {
                    if (container[i] is MonoBinder)
                    {
                        container.RemoveAt(i);
                    }
                }
            }

            targets = container.ToArray();
        }

        [BindingProperty]
        [field: Label("Enabled")]
        [field: HideInInspector]
        private void SetEnabled(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].enabled = value;
            }
        }
    }
}
