#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

using UnityEditor;
using TMPro;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class TMP_TextBinderEditor
    {
        [MenuItem("CONTEXT/TMP_Text/Binder")]
        static void BindTMP_Text(MenuCommand command)
        {
            if (command.context is TMP_Text target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(TMP_Text target)
        {
            var comp = Undo.AddComponent<TMP_TextBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
