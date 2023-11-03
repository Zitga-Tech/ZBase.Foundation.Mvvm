using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.Input;
using ZBase.Foundation.Mvvm.Unions;
using ZBase.Foundation.Mvvm.Unions.__Internal.MvvmTest;
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
            InternalUnions.Register();

            var model = new Model();
            var binder = new Binder();

            binder.Context = model;
            binder.SetTargetPropertyName(Binder.BindingProperty_SetIntValue, Model.PropertyName_IntField);
            binder.SetTargetPropertyName(Binder.BindingProperty_SetAValue, Model.PropertyName_A);
            binder.StartListening();

            while (true)
            {
                var key = Console.ReadKey(true);

                switch (key.Key)
                {
                    case ConsoleKey.Spacebar:
                    {
                        model.IntField += 1;
                        break;
                    }

                    case ConsoleKey.A:
                    {
                        model.A = new A { Value = model.IntField += 1 };
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

    public partial class A : IObservableObject, IEquatable<A>
    {
        [ObservableProperty]
        private int _value;

        public bool Equals(A other)
        {
            if (other == null)
            {
                return false;
            }

            return this._value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (obj is A other)
            {
                return this._value == other._value;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return this._value.GetHashCode();
        }
    }

    public partial class Model : IObservableObject, IBindingContext
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsCreated))]
        [NotifyPropertyChangedFor(nameof(IsCreatedNew))]
        [NotifyCanExecuteChangedFor(nameof(UpdateIntCommand))]
        [NotifyCanExecuteChangedFor(nameof(UpdateNothingCommand))]
        private int _intField;

        [ObservableProperty]
        private TimeSpan _time;

        [ObservableProperty]
        private A _a;

        public TypeCode Type { get; }

        public IObservableObject Target => this;

        public bool IsCreated => true;

        public bool IsCreatedNew => false;

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
        private void SetAValue(A value)
        {
            if (value == null)
            {
                Console.WriteLine("null");
                return;
            }

            Console.WriteLine($"A {value.Value}");
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

    public readonly partial struct Vector3Union : IUnion<Vector3> { }
}
