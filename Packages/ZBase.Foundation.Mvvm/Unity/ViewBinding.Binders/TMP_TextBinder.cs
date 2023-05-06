using TMPro;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders
{
    [AddComponentMenu("MVVM/Binders/TMP Text Binder")]
    [RequireComponent(typeof(TMP_Text))]
    public partial class TMP_TextBinder : MonoBinder
    {
        private TMP_Text _text;

        protected override void OnAwake()
        {
            _text = GetComponent<TMP_Text>();
        }

        [Binding("Text")]
        private void UpdateText(string value)
        {
            _text.text = value;
        }
    }
}