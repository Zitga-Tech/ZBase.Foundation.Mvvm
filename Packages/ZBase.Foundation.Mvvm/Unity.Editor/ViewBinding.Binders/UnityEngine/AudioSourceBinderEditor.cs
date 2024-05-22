using UnityEditor;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using UnityEngine;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding
{
    internal static class AudioSourceBinderEditor
    {
        [MenuItem("CONTEXT/AudioSource/Binder For This Component")]
        static void BindAudioSource(MenuCommand command)
        {
            if (command.context is AudioSource target)
            {
                Setup(target);
            }
        }

        public static MonoBehaviour Setup(AudioSource target)
        {
            var comp = Undo.AddComponent<AudioSourceBinder>(target.gameObject);
            MonoBinderEditor.TryResolveNearestContext(comp);
            return comp;
        }
    }
}
