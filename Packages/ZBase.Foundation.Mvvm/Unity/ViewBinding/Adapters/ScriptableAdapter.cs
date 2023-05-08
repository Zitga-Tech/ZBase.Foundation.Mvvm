using UnityEngine;
using ZBase.Foundation.Mvvm.Unions;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Adapters
{
    public abstract class ScriptableAdapter : ScriptableObject, IAdapter
    {
        public abstract Union Convert(in Union union);
    }
}
