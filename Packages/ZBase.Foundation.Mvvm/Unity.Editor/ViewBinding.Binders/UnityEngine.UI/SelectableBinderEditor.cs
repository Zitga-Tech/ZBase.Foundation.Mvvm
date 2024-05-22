#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class SelectableBinderEditor
    {
        [MenuItem("CONTEXT/Selectable/Binder For This Component")]
        static void BindSelectable(MenuCommand command)
        {
            if (command.context is Selectable target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(Selectable target)
        {
            var comp = Undo.AddComponent<SelectableBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
