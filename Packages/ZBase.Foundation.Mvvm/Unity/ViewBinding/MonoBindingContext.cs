#pragma warning disable CA2201 // Do not raise reserved exception types

using System;
using System.Collections.Generic;
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

        private IObservableObject GetTarget()
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

                    return ResolvePropertyPath(_targetSystemObject, _targetPropertyPath);
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

                    return ResolvePropertyPath(target, _targetPropertyPath);
                }
            }

            ThrowIfTargetKindIsInvalid(_targetKind);
            return null;
        }

        private static IObservableObject ResolvePropertyPath(
              IObservableObject target
            , ReadOnlySpan<string> propertyPath
        )
        {
            if (propertyPath.Length < 1)
            {
                return target;
            }

            var queue = new Queue<string>(propertyPath.Length);

            foreach (var propertyName in propertyPath)
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    ThrowIfCannotResolvePropertyPath();
                    return null;
                }

                queue.Enqueue(propertyName);
            }

            if (target.TryGetMemberObservableObject(queue, out var member))
            {
                return member;
            }

            ThrowIfCannotResolvePropertyPath();
            return null;
        }

        [DoesNotReturn, HideInCallstack]
        private static void ThrowIfTargetSystemObjectIsNull(ContextTargetKind targetKind)
        {
            throw new NullReferenceException(
                $"The target kind is {targetKind} but reference on the `Target System Object` field is null."
            );
        }

        [DoesNotReturn, HideInCallstack]
        private static void ThrowIfTargetUnityObjectIsNull(ContextTargetKind targetKind)
        {
            throw new NullReferenceException(
                $"The target kind is {targetKind} but reference on the `Target Unity Object` field is null."
            );
        }

        [DoesNotReturn, HideInCallstack]
        private static void ThrowIfTargetUnityObjectIsNotObservableObject()
        {
            throw new InvalidCastException(
                $"Reference on the `Target Unity Object` field does not implement {typeof(IObservableObject)}."
            );
        }

        [DoesNotReturn, HideInCallstack]
        private static void ThrowIfTargetKindIsInvalid(ContextTargetKind targetKind)
        {
            throw new InvalidOperationException($"{targetKind} is not a valid target kind.");
        }

        [DoesNotReturn, HideInCallstack]
        private static void ThrowIfCannotResolvePropertyPath()
        {
            throw new InvalidOperationException(
                $"The target {nameof(IObservableObject)} cannot be retrieved by values on the `Target Property Path` field."
            );
        }

        [Serializable]
        public enum ContextTargetKind
        {
            SystemObject = 0,
            UnityObject = 1,
        }
    }
}
