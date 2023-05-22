using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.Input;
using ZBase.Foundation.Mvvm.Unions;
using ZBase.Foundation.Mvvm.Unity.ViewBinding;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace Mvvm.Samples
{
    public partial class ViewModel : MonoBehaviour, IObservableObject
    {
        [ObservableProperty]
        private float _time;

        [ObservableProperty]
        private Vector3 _vector2;

        private bool _updating;

        private void Awake()
        {
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _updating = !_updating;
            }

            if (_updating)
            {
                Time = UnityEngine.Time.time;
            }
        }

        [RelayCommand]
        private void OnToggleValueChanged(bool value)
        {
            Debug.Log(value);
        }
    }
}
