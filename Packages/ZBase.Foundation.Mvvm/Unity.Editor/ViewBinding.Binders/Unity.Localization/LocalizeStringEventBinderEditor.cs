#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class LocalizeStringEventBinderEditor
    {
        [MenuItem("CONTEXT/LocalizeStringEvent/Binder")]
        static void BindLocalizeStringEvent(MenuCommand command)
        {
            if (command.context is LocalizeStringEvent target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(LocalizeStringEvent target)
        {
            var comp = Undo.AddComponent<LocalizeStringEventBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
