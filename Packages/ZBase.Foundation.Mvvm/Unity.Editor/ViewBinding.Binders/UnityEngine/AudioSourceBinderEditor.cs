#if UNITY_TEXT_MESH_PRO || (UNITY_2023_2_OR_NEWER && UNITY_UGUI)

using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class AudioSourceBinderEditor
    {
        [MenuItem("CONTEXT/AudioSource/Binder")]
        static void BindAudioSource(MenuCommand command)
        {
            var target = command.context as AudioSource;
            Setup(target);
        }

        public static MonoBehaviour Setup(AudioSource target)
        {
            var comp = Undo.AddComponent<AudioSourceBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}

#endif
