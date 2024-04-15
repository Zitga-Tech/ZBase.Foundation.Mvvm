#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class RawImageBinderEditor
    {
        [MenuItem("CONTEXT/RawImage/Binder")]
        static void BindRawImage(MenuCommand command)
        {
            if (command.context is RawImage target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(RawImage target)
        {
            var comp = Undo.AddComponent<RawImageBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
