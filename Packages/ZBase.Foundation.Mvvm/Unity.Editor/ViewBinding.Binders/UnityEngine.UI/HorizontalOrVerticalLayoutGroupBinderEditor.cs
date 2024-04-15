#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class HorizontalOrVerticalLayoutGroupBinderEditor
    {
        [MenuItem("CONTEXT/HorizontalOrVerticalLayoutGroup/Binder")]
        static void BindHorizontalOrVerticalLayoutGroup(MenuCommand command)
        {
            var target = command.context as HorizontalOrVerticalLayoutGroup;
            Setup(target);
        }

        public static MonoBehaviour Setup(HorizontalOrVerticalLayoutGroup target)
        {
            var comp = Undo.AddComponent<HorizontalOrVerticalLayoutGroupBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
