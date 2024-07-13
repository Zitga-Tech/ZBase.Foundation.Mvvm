using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;
using UnityEngine.Pool;

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
                ErrorContextTargetIsNull(this);
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
                ErrorContextIsNull(this);
                return null;
            }

            if (_context is not IBindingContext context)
            {
                ErrorContextIsInvalid(this);
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
                ErrorContextTargetIsNull(this);
                return;
            }

            StartListening();
        }

        private void FindNearestContext()
        {
            var parent = this.transform;
            var components = ListPool<IBindingContext>.Get();

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

            ListPool<IBindingContext>.Release(components);
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

            ErrorPropertyBindingFailed(bindingProperty, this.Context, this);
        }

        protected virtual void OnBindCommandFailed(BindingCommand bindingCommand)
        {
            if (string.IsNullOrWhiteSpace(bindingCommand?.TargetCommandName))
            {
                return;
            }

            ErrorPropertyCommandFailed(bindingCommand, this.Context, this);
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void ErrorContextIsNull(UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError("Reference on the `Context` field is null.", context);
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void ErrorContextIsInvalid(UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError("Reference on the `Context` field does not implement {typeof(IBindingContext)}.", context);
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void ErrorContextTargetIsNull(UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError("The target of the Context is null.", context);
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        protected static void ErrorPropertyBindingFailed(
              BindingProperty bindingProperty
            , IBindingContext bindingContext
            , UnityEngine.Object context
        )
        {
            var type = bindingContext?.Target?.GetType();

            UnityEngine.Debug.LogError(
                  $"Cannot bind to any property named `{bindingProperty?.TargetPropertyName}` on {type}. " +
                  $"Please verify if {type} contains this property."
                , context
            );
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        protected static void ErrorPropertyCommandFailed(
              BindingCommand bindingCommand
            , IBindingContext bindingContext
            , UnityEngine.Object context
        )
        {
            var type = bindingContext?.Target?.GetType();

            UnityEngine.Debug.LogError(
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