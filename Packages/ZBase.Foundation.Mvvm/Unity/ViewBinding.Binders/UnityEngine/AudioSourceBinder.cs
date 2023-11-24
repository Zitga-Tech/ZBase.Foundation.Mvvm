using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/AudioSource Binder")]
    public partial class AudioSourceBinder : MonoBinder<AudioSource>
    {
        protected sealed override void OnAwake(ref AudioSource[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<AudioSource>(out var target))
                {
                    targets = new AudioSource[] { target };
                }
            }

            if (targets.Length < 1)
            {
                Logger.WarnIfTargetListIsEmpty(this);
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
