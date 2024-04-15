#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class GraphicRaycasterBinderEditor
    {
        [MenuItem("CONTEXT/GraphicRaycaster/Binder")]
        static void BindGraphicRaycaster(MenuCommand command)
        {
            var target = command.context as GraphicRaycaster;
            Setup(target);
        }

        public static MonoBehaviour Setup(GraphicRaycaster target)
        {
            var comp = Undo.AddComponent<GraphicRaycasterBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
