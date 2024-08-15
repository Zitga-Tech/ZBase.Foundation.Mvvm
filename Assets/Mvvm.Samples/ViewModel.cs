using System;
using UnityEngine;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.Input;
using ZBase.Foundation.Mvvm.Unity.ViewBinding.Binders;

#if UNITY_LOCALIZATION
using UnityEngine.Localization;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
#endif

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

#if UNITY_LOCALIZATION
        [ObservableProperty]
        public LocalizedIVariable[] ProgressVariables { get => Get_ProgressVariables(); set => Set_ProgressVariables(value); }
#endif

        [ObservableProperty]
        private bool _updating;

        [ObservableProperty]
        private B _b = new();

        [ObservableProperty]
        private Vector3 _scale;

        public string TimeText => $"Time: {Time}";

        public double Seconds => Time.TotalSeconds;

#if UNITY_LOCALIZATION
        private IntVariable _progressVariable;
        private IntVariable _unitVariable;
        private LocalizedString _localizedUnit;
#endif

        private void Start()
        {
            this.Time = TimeSpan.Zero;
            this.TextColor = Color.white;

#if UNITY_LOCALIZATION
            this.ProgressVariables = new LocalizedIVariable[] {
                new(_progressVariable = new(), "value"),
                new(_localizedUnit = new("L10n", "unit_format") {
                    { "0", _unitVariable = new() }
                }, "unit"),
            };
#endif
        }

        private void OnDestroy()
        {
#if UNITY_LOCALIZATION
            ((IDisposable)_localizedUnit).Dispose();
#endif
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
            var progress = ReduceToUnit((int)value, out var unit);

#if UNITY_LOCALIZATION
            _progressVariable.Value = progress;
            _unitVariable.Value = unit;
#endif
        }

        private static int ReduceToUnit(int value, out int unit)
        {
            if (value < 10)
            {
                unit = 0;
                return value;
            }

            if (value < 20)
            {
                unit = 1;
                return value / 10;
            }

            if (value < 100)
            {
                unit = 2;
                return value / 10;
            }

            if (value < 1000)
            {
                unit = 3;
                return value / 100;
            }

            unit = 4;
            return value / 1000;
        }
    }
}
