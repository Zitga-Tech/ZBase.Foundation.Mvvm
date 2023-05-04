using System;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace MvvmTest
{
    public class Program
    {
        public static void Main()
        {
            var model = new Model();
            var binder = new Binder();

            binder.DataContext = model;
            binder.SetPropertyName(Binder.BindingField_OnUpdate, Model.PropertyName_IntField);
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

    public partial class Model : IObservableObject, IDataContext
    {
        [ObservableProperty]
        private int _intField;

        public TypeCode Type { get; }

        public IObservableObject ViewModel => this;
    }

    public partial class Binder : IBinder
    {
        public IDataContext DataContext { get; set; }

        [Binding]
        private void OnUpdate(int value)
        {
            Console.WriteLine(value);
        }
    }
}
