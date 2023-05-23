using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;
using System.Collections.Generic;

#if CYSHARP_UNITASK
using Cysharp.Threading.Tasks;
#else
using System.Collections;
#endif

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    public abstract partial class MonoBinder : MonoBehaviour, IBinder
    {
        [SerializeField]
        internal BindingContextSetting _contextSetting;

        [SerializeField, HideInInspector]
        internal Component _context;

        public IBindingContext Context { get; private set; }

#if CYSHARP_UNITASK
        protected async void Awake()
        {
            if (_contextSetting == BindingContextSetting.FindWhenPlay)
            {
                FindNearestContext();
            }

            Context = GetContext();

            if (Context != null)
            {
                await UniTask.WaitUntil(() => Context.IsCreated);
            }

            OnAwake();

            if (Context == null)
            {
                return;
            }

            if (Context.Target == null)
            {
                LogWhenContextTargetIsNull();
                return;
            }
            
            StartListening();
        }
#else
        protected void Awake()
        {
            StartCoroutine(AwakeCoroutine(_contextSetting));
        }

        private IEnumerator AwakeCoroutine(BindingContextSetting bindingContextSetting)
        {
            if (bindingContextSetting == BindingContextSetting.FindWhenPlay)
            {
                FindNearestContext();
            }

            Context = GetContext();

            if (Context != null)
            {
                yield return new WaitUntil(() => Context.IsCreated);
            }

            OnAwake();

            if (Context == null)
            {
                yield break;
            }

            if (Context.Target == null)
            {
                LogWhenContextTargetIsNull();
                yield break;
            }

            StartListening();
        }
#endif

        protected virtual void OnAwake() { }

        protected virtual void OnDestroy()
        {
            StopListening();
        }

        private IBindingContext GetContext()
        {
            if (_context == false)
            {
                throw new NullReferenceException(
                    $"Reference on the `Context` field is null."
                );
            }

            if (_context is not IBindingContext context)
            {
                throw new InvalidCastException(
                    $"Reference on the `Context` field does not implement {typeof(IBindingContext)}."
                );
            }

            return context;
        }

        /// <summary>
        /// Initialize this binder manually. The operation consists of these steps:
        /// <list type="number">
        /// <item>Invoke the <see cref="StopListening"/> method</item>
        /// <item>If the <paramref name="context"/> argument is not null, use it</item>
        /// <item>Otherwise, find the nearest <see cref="IBindingContext"/> on the GameObject hierarchy</item>
        /// <item>Invoke the <see cref="StartListening"/> method on this binder</item>
        /// </list>
        /// </summary>
        public virtual void InitializeManually(IBindingContext context = null)
        {
            StopListening();

            if (context == null || context.Target == null)
            {
                FindNearestContext();
                Context = GetContext();
            }
            else
            {
                this.Context = context;
            }

            if (this.Context == null)
            {
                return;
            }

            if (this.Context.Target == null)
            {
                LogWhenContextTargetIsNull();
                return;
            }

            StartListening();
        }

        private void FindNearestContext()
        {
            var parent = this.transform;
            var components = new List<IBindingContext>();

            while (parent)
            {
                components.Clear();
                parent.GetComponents(components);

                if (components.Count > 0)
                {
                    _context = components[0] as Component;
                    return;
                }

                parent = parent.parent;
            }
        }

        protected virtual void OnBindPropertyFailed(BindingProperty bindingProperty)
        {
            if (this.Context?.Target == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(bindingProperty.TargetPropertyName))
            {
                return;
            }

            Debug.LogError(
                $"Cannot bind to any property named `{bindingProperty.TargetPropertyName}` on {this.Context?.Target?.GetType()}. " +
                $"Please verify if the property is declared for the type."
                , this
            );
        }

        protected virtual void OnBindCommandFailed(BindingCommand bindingProperty)
        {
            if (string.IsNullOrWhiteSpace(bindingProperty.TargetCommandName))
            {
                return;
            }

            Debug.LogError(
                $"Cannot bind to any command named `{bindingProperty.TargetCommandName}` on {this.Context?.Target?.GetType()}. " +
                $"Please verify if the command is declared for the type."
                , this
            );
        }

        private void LogWhenContextTargetIsNull()
        {
            Debug.LogError("The target of the Context is null.", this);
        }

        internal enum BindingContextSetting
        {
            PresetOnEditor = 0,
            FindWhenPlay = 1,
        }
    }
}