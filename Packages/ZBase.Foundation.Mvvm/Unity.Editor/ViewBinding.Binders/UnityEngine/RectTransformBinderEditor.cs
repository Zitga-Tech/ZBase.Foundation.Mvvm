#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class RectTransformBinderEditor
    {
        [MenuItem("CONTEXT/RectTransform/Binder")]
        static void BindRectTransform(MenuCommand command)
        {
            var target = command.context as RectTransform;
            Setup(target);
        }

        public static MonoBehaviour Setup(RectTransform target)
        {
            var comp = Undo.AddComponent<RectTransformBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
