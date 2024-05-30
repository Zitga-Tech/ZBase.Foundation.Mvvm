using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/Audio Source Binder")]
    public partial class AudioSourceBinder : MonoBinder<AudioSource>, IBinder
    {
        protected sealed override void OnAwake([NotNull] ref AudioSource[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<AudioSource>(out var target))
                {
                    targets = new AudioSource[] { target };
                }
            }
        }

        [BindingProperty]
        [field: Label("Volume")]
        [field: HideInInspector]
        private void SetVolume(float value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].volume = value;
            }
        }

        [BindingProperty]
        [field: Label("Mute")]
        [field: HideInInspector]
        private void SetMute(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].mute = value;
            }
        }

        [BindingProperty]
        [field: Label("Loop")]
        [field: HideInInspector]
        private void SetLoop(bool value)
        {
            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].loop = value;
            }
        }
    }
}
