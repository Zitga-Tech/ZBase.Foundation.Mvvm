#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class ToggleBinderEditor
    {
        [MenuItem("CONTEXT/Toggle/Binder For This Component")]
        static void BindToggle(MenuCommand command)
        {
            if (command.context is Toggle target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(Toggle target)
        {
            var comp = Undo.AddComponent<ToggleBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
