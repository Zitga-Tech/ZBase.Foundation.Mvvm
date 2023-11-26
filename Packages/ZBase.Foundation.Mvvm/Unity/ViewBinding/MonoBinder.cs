#pragma warning disable CA2201 // Do not raise reserved exception types

using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#if CYSHARP_UNITASK
using Cysharp.Threading.Tasks;
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
                LogWhenContextTargetIsNull(this);
                return;
            }
            
            StartListening();
            RefreshContext();
        }
#else
        protected void Awake()
        {
            StartCoroutine(AwakeCoroutine(_contextSetting));
        }

        private System.Collections.IEnumerator AwakeCoroutine(BindingContextSetting bindingContextSetting)
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
                LogWhenContextTargetIsNull(this);
                yield break;
            }

            StartListening();
            RefreshContext();
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
                ThrowIfContextIsNull();
                return null;
            }

            if (_context is not IBindingContext context)
            {
                ThrowIfContextIsInvalid();
                return null;
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
                LogWhenContextTargetIsNull(this);
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

            if (string.IsNullOrWhiteSpace(bindingProperty?.TargetPropertyName))
            {
                return;
            }

            LogIfPropertyBindingFailed(bindingProperty, this.Context, this);
        }

        protected virtual void OnBindCommandFailed(BindingCommand bindingCommand)
        {
            if (string.IsNullOrWhiteSpace(bindingCommand?.TargetCommandName))
            {
                return;
            }

            LogIfPropertyCommandFailed(bindingCommand, this.Context, this);
        }

        [DoesNotReturn, HideInCallstack]
        private static void ThrowIfContextIsNull()
        {
            throw new NullReferenceException(
                $"Reference on the `Context` field is null."
            );
        }

        [DoesNotReturn, HideInCallstack]
        private static void ThrowIfContextIsInvalid()
        {
            throw new InvalidCastException(
                $"Reference on the `Context` field does not implement {typeof(IBindingContext)}."
            );
        }

        [HideInCallstack]
        private static void LogWhenContextTargetIsNull(UnityEngine.Object context)
        {
            Debug.LogError("The target of the Context is null.", context);
        }

        [HideInCallstack]
        protected static void LogIfPropertyBindingFailed(
              BindingProperty bindingProperty
            , IBindingContext bindingContext
            , UnityEngine.Object context
        )
        {
            var type = bindingContext?.Target?.GetType();

            Debug.LogError(
                  $"Cannot bind to any property named `{bindingProperty?.TargetPropertyName}` on {type}. " +
                  $"Please verify if {type} contains this property."
                , context
            );
        }

        [HideInCallstack]
        protected static void LogIfPropertyCommandFailed(
              BindingCommand bindingCommand
            , IBindingContext bindingContext
            , UnityEngine.Object context
        )
        {
            var type = bindingContext?.Target?.GetType();

            Debug.LogError(
                  $"Cannot bind to any command named `{bindingCommand?.TargetCommandName}` on {type}. " +
                  $"Please verify if {type} contains this command."
                , context
            );
        }

        internal enum BindingContextSetting
        {
            PresetOnEditor = 0,
            FindWhenPlay = 1,
        }
    }
}