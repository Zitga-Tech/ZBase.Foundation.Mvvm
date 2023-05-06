using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    public abstract partial class MonoBinder : MonoBehaviour, IBinder
    {
        [SerializeField]
        internal Component _context;

        public IBindingContext Context { get; private set; }

        protected void Awake()
        {
            Context = GetContext();

            OnAwake();
        }

        protected virtual void OnAwake() { }

        protected virtual void Start()
        {
            StartListening();
        }

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
    }
}