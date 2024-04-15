#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class LocalizeAudioClipEventBinderEditor
    {
        [MenuItem("CONTEXT/LocalizeAudioClipEvent/Binder")]
        static void BindLocalizeAudioClipEvent(MenuCommand command)
        {
            if (command.context is LocalizeAudioClipEvent target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(LocalizeAudioClipEvent target)
        {
            var comp = Undo.AddComponent<LocalizeAudioClipEventBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
