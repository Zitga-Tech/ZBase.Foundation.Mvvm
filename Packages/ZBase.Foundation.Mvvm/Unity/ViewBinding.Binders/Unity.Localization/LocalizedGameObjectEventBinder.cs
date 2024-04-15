#if UNITY_LOCALIZATION

#pragma warning disable IDE0051 // Remove unused private members

using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Events;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/LocalizedGameObjectEvent Binder")]
    public partial class LocalizedGameObjectEventBinder
        : LocalizedAssetEventBinder<LocalizedGameObjectEvent, GameObject, LocalizedGameObject, UnityEventGameObject>
    {
        [BindingProperty]
        [field: Label("Asset Reference")]
        [field: HideInInspector]
        private void SetAssetReference(LocalizedGameObject value)
        {
            SetAssetReferenceInternal(value);
        }
    }
}

#endif
