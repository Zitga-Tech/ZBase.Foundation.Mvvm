using UnityEngine;
using ZBase.Foundation.Mvvm.Unions;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Adapters;

namespace Mvvm.Samples
{
    [CreateAssetMenu(fileName = "FloatToStringScriptableAdapter", menuName = "MVVM/Adapters/Float => String")]
    public sealed class FloatToStringScriptableAdapter : ScriptableAdapterAsset
    {
        public override Union Convert(in Union union)
        {
            return $"SO: {union.Float}";
        }
    }
}