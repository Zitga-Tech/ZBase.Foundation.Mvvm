using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.Unions;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Adapters
{
    [Serializable]
    [Label("Scriptable Adapter", "Default")]
    public sealed class AdapterUsingScriptableAdapter : IAdapter
    {
        [SerializeField, HideInInspector]
        private ScriptableAdapter _scriptableAdapter;

        public Union Convert(in Union union)
        {
            if (_scriptableAdapter)
            {
                return _scriptableAdapter.Convert(union);
            }

            return union;
        }
    }
}
