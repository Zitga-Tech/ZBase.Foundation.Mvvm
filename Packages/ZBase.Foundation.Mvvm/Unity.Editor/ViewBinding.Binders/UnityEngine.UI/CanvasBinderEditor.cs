using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class CanvasGroupBinderEditor
    {
        [MenuItem("CONTEXT/CanvasGroup/Binder For This Component")]
        static void BindCanvasGroup(MenuCommand command)
        {
            if (command.context is CanvasGroup target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(CanvasGroup target)
        {
            var comp = Undo.AddComponent<CanvasGroupBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}
