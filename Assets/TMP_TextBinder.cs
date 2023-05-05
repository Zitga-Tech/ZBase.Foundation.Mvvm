using TMPro;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace MvvmTests
{
    [RequireComponent(typeof(TMP_Text))]
    public partial class TMP_TextBinder : MonoBehaviour, IBinder
    {
        private TMP_Text _text;

        public IObservableContext Context { get; private set; }

        private void Awake()
        {
            Context = GetComponentInParent<IObservableContext>(true);

            _text = GetComponent<TMP_Text>();
        }

        [Binding("Text Field", "Text Converter")]
        private void UpdateText(string value)
        {
            _text.text = value;
        }
    }
}