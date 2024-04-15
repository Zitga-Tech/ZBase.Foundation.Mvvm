﻿using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class CanvasBinderEditor
    {
        [MenuItem("CONTEXT/Canvas/Binder")]
        static void BindCanvas(MenuCommand command)
        {
            var target = command.context as Canvas;
            Setup(target);
        }

        public static MonoBehaviour Setup(Canvas target)
        {
            var comp = Undo.AddComponent<CanvasBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}