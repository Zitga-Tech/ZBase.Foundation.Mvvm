#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)
#if UNITY_LOCALIZATION

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class LocalizeSpriteEventBinderEditor
    {
        [MenuItem("CONTEXT/LocalizeSpriteEvent/Binder For This Component")]
        static void BindLocalizeSpriteEvent(MenuCommand command)
        {
            if (command.context is LocalizeSpriteEvent target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(LocalizeSpriteEvent target)
        {
            var comp = Undo.AddComponent<LocalizeSpriteEventBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
#endif
