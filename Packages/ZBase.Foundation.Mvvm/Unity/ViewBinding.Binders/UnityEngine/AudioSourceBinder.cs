using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/AudioSource Binder")]
    public partial class AudioSourceBinder : MonoBinder
    {
        [SerializeField]
        private AudioSource[] _targets = new AudioSource[0];

        protected override void OnAwake()
        {
            if (_targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<AudioSource>(out var target))
                {
                    _targets = new AudioSource[] { target };
                }
            }

            if (_targets.Length < 1)
            {
                Debug.LogWarning($"The target list is empty.", this);
            }
        }

        [BindingProperty]
        [field: Label("Volume")]
        [field: HideInInspector]
        private void SetVolume(float value)
        {
            var targets = _targets.AsSpan();
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
            var targets = _targets.AsSpan();
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
            var targets = _targets.AsSpan();
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                targets[i].loop = value;
            }
        }
    }
}
