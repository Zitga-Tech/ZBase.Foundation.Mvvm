using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;

namespace Mvvm.Samples
{
    public partial class ViewModel : MonoBehaviour, IObservableObject
    {
        [ObservableProperty]
        private string _text;

        private bool _updating;

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _updating = !_updating;
            }

            if (_updating)
            {
                Text = Time.time.ToString();
            }
        }
    }
}
