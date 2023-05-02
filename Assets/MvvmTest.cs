using System;
using System.Runtime.InteropServices;
using UnityEngine;
using ZBase.Foundation.Mvvm;
using ZBase.Foundation.Unions;

namespace MvvmTests
{
    public class MvvmTest : MonoBehaviour
    {
        private void Awake()
        {
        }
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
