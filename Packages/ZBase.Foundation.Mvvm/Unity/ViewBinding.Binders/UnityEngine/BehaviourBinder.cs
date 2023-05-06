using System;
using System.Collections.Generic;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Behaviour Binder")]
    public partial class BehaviourBinder : MonoBinder
    {
        [SerializeField]
        private Behaviour[] _targets = new Behaviour[0];

        [SerializeField]
        private bool _includeMonoBinders = false;

        protected override void OnAwake()
        {
            var container = new List<Behaviour>();
            container.AddRange(_targets);

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

            _targets = container.ToArray();

            if (_targets.Length < 1)
            {
                Debug.LogWarning($"The target list is empty.", this);
            }
        }

        [Binding("Enabled")]
        [field: HideInInspector]
        private void SetEnabled(bool value)
        {
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];

                if (target)
                {
                    target.enabled = value;
                }
            }
        }
    }
}
