#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class ImageBinderEditor
    {
        [MenuItem("CONTEXT/Image/Binder For This Component")]
        static void BindImage(MenuCommand command)
        {
            if (command.context is Image target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(Image target)
        {
            var comp = Undo.AddComponent<ImageBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
