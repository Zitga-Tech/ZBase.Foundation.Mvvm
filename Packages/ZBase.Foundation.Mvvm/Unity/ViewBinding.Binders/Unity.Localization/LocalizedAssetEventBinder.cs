#if UNITY_LOCALIZATION

using System.Diagnostics.CodeAnalysis;
using UnityEngine.Events;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    public abstract partial class LocalizedAssetEventBinder<TLocalize, TObject, TReference, TEvent> : MonoBinder<TLocalize>, IBinder
        where TLocalize : LocalizedAssetEvent<TObject, TReference, TEvent>
        where TObject : UnityEngine.Object
        where TReference : LocalizedAsset<TObject>, new()
        where TEvent : UnityEvent<TObject>, new()
    {
        protected override void OnAwake([NotNull] ref TLocalize[] targets)
        {
            if (targets.Length < 1)
            {
                if (this.gameObject.TryGetComponent<TLocalize>(out var target))
                {
                    targets = new TLocalize[] { target };
                }
            }
        }

        protected void SetAssetReferenceInternal(TReference value)
        {
            if (value == null)
            {
                return;
            }

            var targets = Targets.Span;
            var length = targets.Length;

            for (var i = 0; i < length; i++)
            {
                var target = targets[i];
                target.AssetReference = value;
            }
        }
    }
}

#endif
