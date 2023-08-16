using System;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    public sealed class MonoBindingContext : MonoBehaviour, IBindingContext
    {
        [SerializeField, HideInInspector]
        internal ContextTargetKind _targetKind;

        [SerializeField, SerializeReference, HideInInspector]
        internal IObservableObject _targetSystemObject;

        [SerializeField, HideInInspector]
        internal UnityEngine.Object _targetUnityObject;

        [SerializeField, HideInInspector]
        internal string[] _targetPropertyPath;

        public bool IsCreated { get; private set; }

        public IObservableObject Target { get; private set; }

        private void Awake()
        {
            Target = GetTarget();
            IsCreated = true;
        }

        internal IObservableObject GetTarget()
        {
            switch (_targetKind)
            {
                case ContextTargetKind.SystemObject:
                {
                    if (_targetSystemObject == null)
                    {
                        ThrowIfTargetSystemObjectIsNull(_targetKind);
                        return null;
                    }

                    return _targetSystemObject;
                }

                case ContextTargetKind.UnityObject:
                {
                    if (_targetUnityObject == false)
                    {
                        ThrowIfTargetUnityObjectIsNull(_targetKind);
                        return null;
                    }

                    if (_targetUnityObject is not IObservableObject target)
                    {
                        ThrowIfTargetUnityObjectIsNotObservableObject();
                        return null;
                    }

                    return target;
                }
            }

            ThrowIfTargetKindIsInvalid(_targetKind);
            return null;
        }

        [DoesNotReturn]
        private static void ThrowIfTargetSystemObjectIsNull(ContextTargetKind targetKind)
        {
            throw new NullReferenceException(
                $"The target kind is {targetKind} but reference on the `Target System Object` field is null."
            );
        }

        [DoesNotReturn]
        private static void ThrowIfTargetUnityObjectIsNull(ContextTargetKind targetKind)
        {
            throw new NullReferenceException(
                $"The target kind is {targetKind} but reference on the `Target Unity Object` field is null."
            );
        }

        [DoesNotReturn]
        private static void ThrowIfTargetUnityObjectIsNotObservableObject()
        {
            throw new InvalidCastException(
                $"Reference on the `Target Unity Object` field does not implement {typeof(IObservableObject)}."
            );
        }

        [DoesNotReturn]
        private static void ThrowIfTargetKindIsInvalid(ContextTargetKind targetKind)
        {
            throw new InvalidOperationException($"{targetKind} is not a valid target kind.");
        }

        [Serializable]
        public enum ContextTargetKind
        {
            SystemObject = 0,
            UnityObject = 1,
        }
    }
}
