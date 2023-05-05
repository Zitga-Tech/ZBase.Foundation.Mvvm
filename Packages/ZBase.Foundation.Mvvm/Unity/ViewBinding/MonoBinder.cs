using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    public abstract partial class MonoBinder : MonoBehaviour, IBinder
    {
        [SerializeField]
        internal Component _context;

        public IObservableContext Context { get; private set; }

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

        private IObservableContext GetContext()
        {
            if (_context == false)
            {
                throw new NullReferenceException(
                    $"Reference on the `Context` field is null."
                );
            }

            if (_context is not IObservableContext context)
            {
                throw new InvalidCastException(
                    $"Reference on the `Context` field does not implement {typeof(IObservableContext)}."
                );
            }

            return context;
        }
    }
}