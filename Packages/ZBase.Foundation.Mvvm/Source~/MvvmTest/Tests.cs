using System;
using System.Runtime.CompilerServices;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.Input;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace MvvmTest
{
    public class Program
    {
        public static void PrintSize<T>()
        {
            Console.WriteLine(Unsafe.SizeOf<T>());
        }

        public static void Main()
        {
            var model = new Model();
            var binder = new Binder();

            binder.Context = model;
            //binder.SetTargetPropertyName(Binder.BindingProperty_OnUpdate, Model.PropertyName_IntField);
            binder.StartListening();

            while (true)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.Spacebar:
                    {
                        model.IntField += 1;
                        break;
                    }

                    case ConsoleKey.Enter:
                    {
                        binder.StartListening();
                        break;
                    }

                    case ConsoleKey.Backspace:
                    {
                        binder.StopListening();
                        break;
                    }

                    default:
                        return;
                }
            }
        }
    }

    public class A { }

    public partial class Model : IObservableObject, IBindingContext
    {
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UpdateIntCommand))]
        private int _intField;

        public TypeCode Type { get; }

        public IObservableObject Target => this;

        public bool IsCreated => true;

        [RelayCommand]
        private void UpdateNothing()
        {
        }

        [RelayCommand]
        private void UpdateInt(int value)
        {

        }
    }

    public partial class Binder : IBinder
    {
        public IBindingContext Context { get; set; }

        [BindingProperty]
        private void SetIntValue(int value)
        {
            Console.WriteLine(value);
        }

        [BindingProperty]
        private void SetTypeCode(ref TypeCode value)
        {

        }

        [BindingCommand]
        partial void OnValueChanged();

        [BindingCommand]
        partial void OnBoolValueChanged(ref bool value);
    }
}
