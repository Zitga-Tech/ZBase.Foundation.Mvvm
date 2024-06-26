﻿#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class ButtonBinderEditor
    {
        [MenuItem("CONTEXT/Button/Binder For This Component")]
        static void BindButton(MenuCommand command)
        {
            if (command.context is Button target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(Button target)
        {
            var comp = Undo.AddComponent<ButtonBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
