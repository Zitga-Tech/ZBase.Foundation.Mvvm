#if UNITY_LOCALIZATION

#pragma warning disable IDE0051 // Remove unused private members

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/LocalizeTextureEvent Binder")]
    public partial class LocalizeTextureEventBinder
        : LocalizedAssetEventBinder<LocalizeTextureEvent, Texture, LocalizedTexture, UnityEventTexture>
    {
        [BindingProperty]
        [field: Label("Reference")]
        [field: HideInInspector]
        private void SetAssetReference(LocalizedTexture value)
        {
            SetAssetReferenceInternal(value);
        }
    }
}

#endif
