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
        internal ContextTargetKind _targetKind = ContextTargetKind.UnityObject;

        [SerializeField, SerializeReference, HideInInspector]
        internal IObservableObject _targetSystemObject;

        [SerializeField, HideInInspector]
        internal UnityEngine.Object _targetUnityObject;

        [SerializeField, HideInInspector]
        internal string[] _targetPropertyPath;

        [SerializeField, HideInInspector]
        internal bool _createOnAwake = true;

        public bool IsCreated { get; private set; }

        public IObservableObject Target { get; private set; }

        private void Awake()
        {
            if (_createOnAwake == false)
            {
                return;
            }

            if (TryGetTarget(out var target))
            {
                Target = target;
                IsCreated = true;
            }
        }

        private bool TryGetTarget(out IObservableObject result)
        {
            switch (_targetKind)
            {
                case ContextTargetKind.SystemObject:
                {
                    if (_targetSystemObject == null)
                    {
                        LogIfTargetSystemObjectIsNull(_targetKind);
                        result = null;
                        return false;
                    }

                    return TryResolvePropertyPath(_targetSystemObject, _targetPropertyPath, out result);
                }

                case ContextTargetKind.UnityObject:
                {
                    if (_targetUnityObject == false)
                    {
                        LogIfTargetUnityObjectIsNull(_targetKind);
                        result = null;
                        return false;
                    }

                    if (_targetUnityObject is not IObservableObject target)
                    {
                        LogIfTargetUnityObjectIsNotObservableObject();
                        result = null;
                        return false;
                    }

                    return TryResolvePropertyPath(target, _targetPropertyPath, out result);
                }
            }

            LogIfTargetKindIsInvalid(_targetKind);
            result = null;
            return false;
        }

        public bool InitializeManually(IObservableObject target)
        {
            if (target is UnityEngine.Object targetUnityObject)
            {
                _targetKind = ContextTargetKind.UnityObject;
                _targetUnityObject = targetUnityObject;
                _targetSystemObject = null;

                if (TryResolvePropertyPath(target, _targetPropertyPath, out var resolvedTarget))
                {
                    Target = resolvedTarget;
                    return IsCreated = true;
                }
            }
            else if (target is IObservableObject targetSystemObject)
            {
                _targetKind = ContextTargetKind.SystemObject;
                _targetSystemObject = targetSystemObject;
                _targetUnityObject = null;

                if (TryResolvePropertyPath(target, _targetPropertyPath, out var resolvedTarget))
                {
                    Target = resolvedTarget;
                    return IsCreated = true;
                }
            }

            LogIfTargetArgumentIsNull();
            return false;
        }

        private static bool TryResolvePropertyPath(
              IObservableObject target
            , ReadOnlySpan<string> propertyPath
            , out IObservableObject result
        )
        {
            if (propertyPath.Length < 1)
            {
                result = target;
                return true;
            }

            var queue = new Queue<string>(propertyPath.Length);

            foreach (var propertyName in propertyPath)
            {
                if (string.IsNullOrEmpty(propertyName))
                {
                    LogIfCannotResolvePropertyPath();
                    result = null;
                    return false;
                }

                queue.Enqueue(propertyName);
            }

            if (target.TryGetMemberObservableObject(queue, out var member))
            {
                result = member;
                return true;
            }

            LogIfCannotResolvePropertyPath();
            result = null;
            return false;
        }

        [DoesNotReturn, HideInCallstack]
        private static void LogIfTargetArgumentIsNull()
        {
            Debug.LogError("The `target` argument is null.");
        }

        [DoesNotReturn, HideInCallstack]
        private static void LogIfTargetSystemObjectIsNull(ContextTargetKind targetKind)
        {
            Debug.LogError(
                $"The target kind is {targetKind} but reference on the `Target System Object` field is null."
            );
        }

        [DoesNotReturn, HideInCallstack]
        private static void LogIfTargetUnityObjectIsNull(ContextTargetKind targetKind)
        {
            Debug.LogError(
                $"The target kind is {targetKind} but reference on the `Target Unity Object` field is null."
            );
        }

        [DoesNotReturn, HideInCallstack]
        private static void LogIfTargetUnityObjectIsNotObservableObject()
        {
            Debug.LogError(
                $"Reference on the `Target Unity Object` field does not implement {typeof(IObservableObject)}."
            );
        }

        [DoesNotReturn, HideInCallstack]
        private static void LogIfTargetKindIsInvalid(ContextTargetKind targetKind)
        {
            Debug.LogError($"{targetKind} is not a valid target kind.");
        }

        [DoesNotReturn, HideInCallstack]
        private static void LogIfCannotResolvePropertyPath()
        {
            Debug.LogError(
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
