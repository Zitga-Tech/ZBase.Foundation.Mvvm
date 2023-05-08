using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.Unions;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Adapters
{
    [Serializable]
    [Label("Scriptable Adapter", "Default")]
    public sealed class ScriptableAdapter : IAdapter
    {
        [SerializeField, HideInInspector]
        private ScriptableAdapterAsset _asset;

        public Union Convert(in Union union)
        {
            if (_asset)
            {
                return _asset.Convert(union);
            }

            return union;
        }
    }
}
