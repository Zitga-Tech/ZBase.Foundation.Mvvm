using System;
using ZBase.Foundation.Mvvm.ComponentModel;
using ZBase.Foundation.Mvvm.Input;
using ZBase.Foundation.Mvvm.Unions;
using ZBase.Foundation.Mvvm.ViewBinding;

namespace MvvmTest
{
    public class Program
    {
        public static void Main()
        {

        }
    }

    public partial class Model : IObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Index))]
        [property: Custom]
        private int _intField;

        [ObservableProperty]
        private TypeCode _typeCode;

        public int Index { get; }

        [RelayCommand]
        [field: Custom2(typeof(int))]
        [property: Custom2(typeof(int), AdditionType2 = typeof(float))]
        private void Process(TypeCode code)
        {

        }
    }

    public partial struct UnionTypCode : IUnion<TypeCode> { }

    [AttributeUsage(AttributeTargets.All)]
    public sealed class CustomAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.All)]
    public sealed class Custom2Attribute : Attribute
    {
        public Type AdditionType2 { get; set; }

        public Custom2Attribute(Type type, Type additionType1 = null) { }
    }

    public partial class Binder : IBinder
    {
        public IDataContext DataContext { get; set; }

        [Binding]
        private void OnUpdate(in Union value) { }
    }

    public partial class XBinder : Binder
    {
        [Binding]
        private void OnNewUpdate(in Union value) { }
    }
}
