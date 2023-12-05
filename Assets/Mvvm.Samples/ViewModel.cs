using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.Input;

namespace Mvvm.Samples
{
    public partial class A : IObservableObject
    {
        [ObservableProperty]
        public float X
        {
            get => Get_X();
            set => Set_X(value);
        }
    }

    public partial class B : IObservableObject
    {
        [ObservableProperty]
        private A _a = new();
    }

    public partial class ViewModel : MonoBehaviour, IObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(TimeText))]
        [NotifyPropertyChangedFor(nameof(Seconds))]
        private TimeSpan _time;

        [ObservableProperty]
        private Color _textColor;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ProgressText))]
        public float Progress
        {
            get => Get_Progress();
            set => Set_Progress(value);
        }

        [ObservableProperty]
        private bool _updating;

        [ObservableProperty]
        private B _b = new();

        [ObservableProperty]
        private Vector3 _scale;

        public string TimeText => $"Time: {Time}";

        public double Seconds => Time.TotalSeconds;

        public string ProgressText => $"{Progress * 100f} %";

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

                var rnd = UnityEngine.Random.Range(0, 1f);
                Scale = Vector3.one * rnd;
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
