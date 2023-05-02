using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using X.TY.Z;

namespace MvvmToolkit
{
    public static class Program
    {
        public static void Main()
        {
        }
    }

    [ObservableObject]
    public partial class ModelA
    {
        [ObservableProperty]
        private int _age;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        [NotifyCanExecuteChangedFor(nameof(GreetUserCommand))]
        private string? _firstName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        [NotifyCanExecuteChangedFor(nameof(GreetUserCommand))]
        private string? _lastName;

        [ObservableProperty]
        private MyStr<MyEnum> _customField;

        private string? FullName => $"{FirstName} {LastName}";

        [RelayCommand]
        private static void GreetUser(ModelA x)
        {
            Console.WriteLine($"Hello {x.FullName}");
        }

        private static bool Validate() => false;

        [RelayCommand(CanExecute = nameof(Validate))]
        private void DoX(ModelA x)
        {
        }
    }

    [ObservableObject]
    public partial class ModelB
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private TypeCode _type;
    }

    public enum MyEnum { A, B }
}

namespace X.TY.Z
{
    public struct MyStr<T> { }
}