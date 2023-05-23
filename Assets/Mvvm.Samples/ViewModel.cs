using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.Input;

namespace Mvvm.Samples
{
    public partial class ViewModel : MonoBehaviour, IObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TimeText))]
        private float _time;

        [ObservableProperty]
        private Color _textColor;

        [ObservableProperty]
        private float _progress;

        private bool _updating;

        public string TimeText => $"Time: {Time}";

        private void Start()
        {
            this.Time = 0f;
            this.TextColor = Color.white;
            this.Progress = 0f;
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
        private void OnSetRedToggle(bool value)
        {
            this.TextColor = value ? Color.red : Color.white;
        }

        [RelayCommand]
        private void OnStartClick()
        {
            _updating = !_updating;
        }

        [RelayCommand]
        private void OnSetProgress(float value)
        {
            this.Progress = value;
        }
    }
}
