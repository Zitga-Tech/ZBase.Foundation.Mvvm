#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.Localization.Components;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class LocalizedGameObjectEventBinderEditor
    {
        [MenuItem("CONTEXT/LocalizedGameObjectEvent/Binder")]
        static void BindLocalizedGameObjectEvent(MenuCommand command)
        {
            if (command.context is LocalizedGameObjectEvent target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(LocalizedGameObjectEvent target)
        {
            var comp = Undo.AddComponent<LocalizedGameObjectEventBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
