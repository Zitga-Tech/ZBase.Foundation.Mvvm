using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                        ErrorTargetSystemObjectIsNull(_targetKind, this);
                        result = null;
                        return false;
                    }

                    return TryResolvePropertyPath(_targetSystemObject, _targetPropertyPath, out result, this);
                }

                case ContextTargetKind.UnityObject:
                {
                    if (_targetUnityObject == false)
                    {
                        ErrorTargetUnityObjectIsNull(_targetKind, this);
                        result = null;
                        return false;
                    }

                    if (_targetUnityObject is not IObservableObject target)
                    {
                        ErrorTargetUnityObjectIsNotObservableObject(this);
                        result = null;
                        return false;
                    }

                    return TryResolvePropertyPath(target, _targetPropertyPath, out result, this);
                }
            }

            ErrorTargetKindIsInvalid(_targetKind, this);
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

                if (TryResolvePropertyPath(target, _targetPropertyPath, out var resolvedTarget, this))
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

                if (TryResolvePropertyPath(target, _targetPropertyPath, out var resolvedTarget, this))
                {
                    Target = resolvedTarget;
                    return IsCreated = true;
                }
            }

            ErrorTargetArgumentIsNull(this);
            return false;
        }

        private static bool TryResolvePropertyPath(
              IObservableObject target
            , ReadOnlySpan<string> propertyPath
            , out IObservableObject result
            , UnityEngine.Object context
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
                    ErrorCannotResolvePropertyPath(context);
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

            ErrorCannotResolvePropertyPath(context);
            result = null;
            return false;
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void ErrorTargetArgumentIsNull(UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError("The `target` argument is null.", context);
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void ErrorTargetSystemObjectIsNull(ContextTargetKind targetKind, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError(
                  $"The target kind is {targetKind} but reference on the `Target System Object` field is null."
                , context
            );
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void ErrorTargetUnityObjectIsNull(ContextTargetKind targetKind, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError(
                  $"The target kind is {targetKind} but reference on the `Target Unity Object` field is null."
                , context
            );
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void ErrorTargetUnityObjectIsNotObservableObject(UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError(
                  $"Reference on the `Target Unity Object` field does not implement {typeof(IObservableObject)}."
                , context
            );
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void ErrorTargetKindIsInvalid(ContextTargetKind targetKind, UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError($"{targetKind} is not a valid target kind.", context);
        }

        [HideInCallstack, DoesNotReturn, Conditional("UNITY_EDITOR"), Conditional("DEVELOPMENT_BUILD")]
        private static void ErrorCannotResolvePropertyPath(UnityEngine.Object context)
        {
            UnityEngine.Debug.LogError(
                  $"The target {nameof(IObservableObject)} cannot be retrieved by values on the `Target Property Path` field."
                , context
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
