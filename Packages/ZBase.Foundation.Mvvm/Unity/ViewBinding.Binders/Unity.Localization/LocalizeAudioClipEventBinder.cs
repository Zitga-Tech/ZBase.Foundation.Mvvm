#if UNITY_LOCALIZATION

#pragma warning disable IDE0051 // Remove unused private members

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/LocalizeAudioClipEvent Binder")]
    public partial class LocalizeAudioClipEventBinder
        : LocalizedAssetEventBinder<LocalizeAudioClipEvent, AudioClip, LocalizedAudioClip, UnityEventAudioClip>
    {
        [BindingProperty]
        [field: Label("Asset Reference")]
        [field: HideInInspector]
        private void SetAssetReference(LocalizedAudioClip value)
        {
            SetAssetReferenceInternal(value);
        }
    }
}

#endif
