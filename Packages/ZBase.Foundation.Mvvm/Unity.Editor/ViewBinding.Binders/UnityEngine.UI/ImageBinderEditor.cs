#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class ImageBinderEditor
    {
        [MenuItem("CONTEXT/Image/Binder")]
        static void BindImage(MenuCommand command)
        {
            var target = command.context as Image;
            Setup(target);
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
