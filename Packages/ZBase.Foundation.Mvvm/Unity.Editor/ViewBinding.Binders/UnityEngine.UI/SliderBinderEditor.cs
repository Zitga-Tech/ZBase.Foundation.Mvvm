#if UNITY_UGUI

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;
using UnityEngine.UI;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class SliderBinderEditor
    {
        [MenuItem("CONTEXT/Slider/Binder For This Component")]
        static void BindSlider(MenuCommand command)
        {
            if (command.context is Slider target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(Slider target)
        {
            var comp = Undo.AddComponent<SliderBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
