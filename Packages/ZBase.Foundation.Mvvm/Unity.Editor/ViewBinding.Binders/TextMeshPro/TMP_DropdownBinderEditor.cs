#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

using UnityEditor;
using TMPro;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class TMP_DropdownBinderEditor
    {
        [MenuItem("CONTEXT/TMP_Dropdown/Binder")]
        static void BindTMP_Dropdown(MenuCommand command)
        {
            if (command.context is TMP_Dropdown target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(TMP_Dropdown target)
        {
            var comp = Undo.AddComponent<TMP_DropdownBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
