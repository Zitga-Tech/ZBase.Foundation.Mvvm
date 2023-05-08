using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ViewBinding.Adapters
{
    [Serializable]
    [Label("Composite Adapter", "Default")]
    public sealed class CompositeAdapter : IAdapter
    {
#if UNITY_5_3_OR_NEWER
        [UnityEngine.SerializeField]
        [UnityEngine.SerializeReference]
        [UnityEngine.HideInInspector]
#endif
        private List<IAdapter> _adapters = new List<IAdapter>();

        public IReadOnlyList<IAdapter> Adapters
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _adapters;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Insert(int index, IAdapter adapter)
            => _adapters.Insert(index, adapter);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void RemoveAt(int index)
            => _adapters.RemoveAt(index);

        public Union Convert(in Union union)
        {
            var adapters = _adapters;
            var length = adapters.Count;
            var result = union;

            for (var i = 0; i < length; i++)
            {
                result = adapters[i].Convert(result);
            }

            return result;
        }
    }
}
