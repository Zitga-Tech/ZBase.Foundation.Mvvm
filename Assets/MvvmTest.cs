using System;
using UnityEngine;
using ZBase.Foundation.Mvvm;

namespace MvvmTests
{
    public class MvvmTest : MonoBehaviour
    {
    }

    public partial class MyModel : IObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FloatValue))]
        private int _intValue;

        private int FloatValue => default;

        [RelayCommand]
        private void Work(int value)
        {

        }

        private bool Validate(int x) => false;

        [RelayCommand(CanExecute = nameof(Validate))]
        private void Process(int x)
        {

        }
    }

    public partial class MyModelX : IObservableObject
    {
        [ObservableProperty]
        private int _intValue;

        [ObservableProperty]
        private string _stringValue;
    }
}
