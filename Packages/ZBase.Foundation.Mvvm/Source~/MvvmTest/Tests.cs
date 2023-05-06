using System;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.Input;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace MvvmTest
{
    public class Program
    {
        public static void Main()
        {
            var model = new Model();
            var binder = new Binder();

            binder.Context = model;
            binder.SetTargetPropertyName(Binder.BindingProperty_OnUpdate, Model.PropertyName_IntField);
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

    public partial class Model : IObservableObject, IBindingContext
    {
        [ObservableProperty]
        private int _intField;

        public TypeCode Type { get; }

        public IObservableObject Target => this;

        [RelayCommand]
        private void UpdateInt(int value)
        {

        }
    }

    public partial class Binder : IBinder
    {
        public IBindingContext Context { get; set; }

        [Binding]
        private void OnUpdate(int value)
        {
            Console.WriteLine(value);
        }
    }
}
