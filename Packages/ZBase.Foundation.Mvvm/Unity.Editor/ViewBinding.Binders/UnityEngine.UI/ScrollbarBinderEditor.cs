#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class ScrollbarBinderEditor
    {
        [MenuItem("CONTEXT/Scrollbar/Binder For This Component")]
        static void BindScrollbar(MenuCommand command)
        {
            if (command.context is Scrollbar target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(Scrollbar target)
        {
            var comp = Undo.AddComponent<ScrollbarBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
