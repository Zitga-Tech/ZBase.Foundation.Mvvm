#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class ScrollRectBinderEditor
    {
        [MenuItem("CONTEXT/ScrollRect/Binder")]
        static void BindScrollRect(MenuCommand command)
        {
            if (command.context is ScrollRect target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(ScrollRect target)
        {
            var comp = Undo.AddComponent<ScrollRectBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
