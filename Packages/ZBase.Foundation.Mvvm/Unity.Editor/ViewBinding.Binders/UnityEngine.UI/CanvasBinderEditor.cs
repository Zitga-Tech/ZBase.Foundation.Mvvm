using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class CanvasGroupBinderEditor
    {
        [MenuItem("CONTEXT/CanvasGroup/Binder")]
        static void BindCanvasGroup(MenuCommand command)
        {
            var target = command.context as CanvasGroup;
            Setup(target);
        }

        public static MonoBehaviour Setup(CanvasGroup target)
        {
            var comp = Undo.AddComponent<CanvasGroupBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}
