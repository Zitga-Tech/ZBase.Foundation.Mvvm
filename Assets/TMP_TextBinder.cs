using TMPro;
using UnityEngine;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace MvvmTests
{
    [RequireComponent(typeof(TMP_Text))]
    public partial class TMP_TextBinder : MonoBehaviour, IBinder
    {
        private TMP_Text _text;

        public IDataContext DataContext { get; private set; }

        private void Awake()
        {
            DataContext = GetComponentInParent<IDataContext>(true);

            _text = GetComponent<TMP_Text>();
        }

        [Binding]
        private void UpdateText(string value)
        {
            _text.text = value;
        }
    }
}