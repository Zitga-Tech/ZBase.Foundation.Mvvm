#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

using UnityEditor;
using TMPro;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class TMP_InputFieldBinderEditor
    {
        [MenuItem("CONTEXT/TMP_InputField/Binder")]
        static void BindTMP_InputField(MenuCommand command)
        {
            if (command.context is TMP_InputField target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(TMP_InputField target)
        {
            var comp = Undo.AddComponent<TMP_InputFieldBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
