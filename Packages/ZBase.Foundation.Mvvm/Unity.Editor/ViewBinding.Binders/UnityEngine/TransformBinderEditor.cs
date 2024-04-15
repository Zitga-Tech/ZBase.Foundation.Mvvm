#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class TransformBinderEditor
    {
        [MenuItem("CONTEXT/Transform/Binder")]
        static void BindTransform(MenuCommand command)
        {
            var target = command.context as Transform;
            Setup(target);
        }

        public static MonoBehaviour Setup(Transform target)
        {
            var comp = Undo.AddComponent<TransformBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
