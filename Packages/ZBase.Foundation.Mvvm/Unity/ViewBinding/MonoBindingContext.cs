using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    public sealed class MonoBindingContext : MonoBehaviour, IBindingContext
    {
        [SerializeField, HideInInspector]
        internal ContextTargetKind _targetKind;

        [SerializeField, HideInInspector]
        [SerializeReference]
        internal IObservableObject _targetSystemObject;

        [SerializeField, HideInInspector]
        internal UnityEngine.Object _targetUnityObject;

        public IObservableObject Target { get; private set; }

        private void Awake()
        {
            Target = GetTarget();
        }

        private IObservableObject GetTarget()
        {
            switch (_targetKind)
            {
                case ContextTargetKind.SystemObject:
                {
                    if (_targetSystemObject == null)
                    {
                        throw new NullReferenceException(
                            $"The target kind is {_targetKind} but reference on the `Target System Object` field is null."
                        );
                    }

                    return _targetSystemObject;
                }

                case ContextTargetKind.UnityObject:
                {
                    if (_targetUnityObject == false)
                    {
                        throw new NullReferenceException(
                            $"The target kind is {_targetKind} but reference on the `Target Unity Object` field is null."
                        );
                    }

                    if (_targetUnityObject is not IObservableObject target)
                    {
                        throw new InvalidCastException(
                            $"Reference on the `Target Unity Object` field does not implement {typeof(IObservableObject)}."
                        );
                    }

                    return target;
                }
            }

            throw new InvalidOperationException($"{_targetKind} is not a valid target kind.");
        }

        [Serializable]
        public enum ContextTargetKind
        {
            SystemObject = 0,
            UnityObject = 1,
        }
    }
}
