#if UNITY_LOCALIZATION

#pragma warning disable IDE0051 // Remove unused private members

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/LocalizeSpriteEvent Binder")]
    public partial class LocalizeSpriteEventBinder
        : LocalizedAssetEventBinder<LocalizeSpriteEvent, Sprite, LocalizedSprite, UnityEventSprite>
    {
        [BindingProperty]
        [field: Label("Asset Reference")]
        [field: HideInInspector]
        private void SetAssetReference(LocalizedSprite value)
        {
            SetAssetReferenceInternal(value);
        }
    }
}

#endif
