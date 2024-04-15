#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class LocalizeTextureEventBinderEditor
    {
        [MenuItem("CONTEXT/LocalizeTextureEvent/Binder")]
        static void BindLocalizeTextureEvent(MenuCommand command)
        {
            if (command.context is LocalizeTextureEvent target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(LocalizeTextureEvent target)
        {
            var comp = Undo.AddComponent<LocalizeTextureEventBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
