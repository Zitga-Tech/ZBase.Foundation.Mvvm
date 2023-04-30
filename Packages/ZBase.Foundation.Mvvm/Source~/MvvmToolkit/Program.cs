using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace MvvmToolkit
{
    public static class Program
    {
        public static void Main()
        {
            var model = new MyViewModel();
            model.PropertyChanged += Model_PropertyChanged;

            var random = new Random();

            while (true)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.Spacebar:
                        model.LastName = random.NextInt64().ToString();
                        break;

                    default: return;
                }
            }
        }

        private static void Model_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (sender is MyViewModel model)
            {
                Console.WriteLine($"Changed: {e.PropertyName}: {model.FullName}");
            }
        }
    }

    [ObservableObject]
    public partial class MyViewModel
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

        public string? FullName => $"{FirstName} {LastName}";

        [RelayCommand]
        private static void GreetUser(MyViewModel x)
        {
            Console.WriteLine($"Hello {x.FullName}");
        }

        private static bool Validate(MyViewModel x) => false;

        [RelayCommand(CanExecute = nameof(Validate))]
        private void DoX(MyViewModel x)
        {
        }
    }
}