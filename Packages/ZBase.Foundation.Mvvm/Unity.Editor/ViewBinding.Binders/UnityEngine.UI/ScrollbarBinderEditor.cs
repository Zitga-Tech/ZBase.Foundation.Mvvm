#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class ScrollbarBinderEditor
    {
        [MenuItem("CONTEXT/Scrollbar/Binder")]
        static void BindScrollbar(MenuCommand command)
        {
            var target = command.context as Scrollbar;
            Setup(target);
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
