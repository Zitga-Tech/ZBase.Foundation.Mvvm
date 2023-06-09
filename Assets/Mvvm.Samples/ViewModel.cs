using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.Input;

namespace Mvvm.Samples
{
    public partial class ViewModel : MonoBehaviour, IObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TimeText))]
        private TimeSpan _time;

        [ObservableProperty]
        private Color _textColor;

        [ObservableProperty]
        private float _progress;

        [ObservableProperty]
        private bool _updating;

        public string TimeText => $"Time: {Time}";

        private void Start()
        {
            this.Time = TimeSpan.Zero;
            this.TextColor = Color.white;
            this.Progress = 0f;
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                Updating = !Updating;
            }

            if (Updating)
            {
                Time = TimeSpan.FromSeconds(UnityEngine.Time.time);
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
            Updating = !Updating;
        }

        [RelayCommand]
        private void OnSetProgress(float value)
        {
            this.Progress = value;
        }
    }
}
