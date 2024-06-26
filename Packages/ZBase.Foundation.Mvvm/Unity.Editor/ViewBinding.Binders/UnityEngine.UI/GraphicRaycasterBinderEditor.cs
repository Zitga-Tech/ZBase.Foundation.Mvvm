﻿#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class GraphicRaycasterBinderEditor
    {
        [MenuItem("CONTEXT/GraphicRaycaster/Binder For This Component")]
        static void BindGraphicRaycaster(MenuCommand command)
        {
            if (command.context is GraphicRaycaster target)
            {
                Setup(target);
            }
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
