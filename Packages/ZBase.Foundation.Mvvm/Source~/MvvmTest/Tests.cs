using ZBase.Foundation.Mvvm;

namespace MvvmTests
{
    public partial class MyViewModel : IObservableObject
    {
        [ObservableProperty]
        private int _age;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        //[NotifyCanExecuteChangedFor(nameof(GreetUserCommand))]
        private string _firstName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        //[NotifyCanExecuteChangedFor(nameof(GreetUserCommand))]
        private string _lastName;

        public string FullName => $"{_firstName} {_lastName}";

        [global::System.CodeDom.Compiler.GeneratedCode("ZBase.Foundation.Mvvm.ObservablePropertyGenerator", "1.0.0")]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public int Age
        {
            get => _age;
            set
            {
                if (global::System.Collections.Generic.EqualityComparer<int>.Default.Equals(_age, value) == false)
                {
                    var notification = new AgeNotification(value);
                    OnPropertyChanging(notification);
                    _age = value;
                    OnPropertyChanged(notification);
                }
            }
        }

        [global::System.CodeDom.Compiler.GeneratedCode("ZBase.Foundation.Mvvm.ObservablePropertyGenerator", "1.0.0")]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (global::System.Collections.Generic.EqualityComparer<string>.Default.Equals(_firstName, value) == false)
                {
                    var notification = new FirstNameNotification(value);
                    OnPropertyChanging(notification);
                    _firstName = value;
                    OnPropertyChanged(notification);
                    OnPropertyChanged(new FullNameNotification(FullName));
                }
            }
        }

        [global::System.CodeDom.Compiler.GeneratedCode("ZBase.Foundation.Mvvm.ObservablePropertyGenerator", "1.0.0")]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public string LastName
        {
            get => _lastName;
            set
            {
                if (global::System.Collections.Generic.EqualityComparer<string>.Default.Equals(_lastName, value) == false)
                {
                    var notification = new LastNameNotification(value);
                    OnPropertyChanging(notification);
                    _lastName = value;
                    OnPropertyChanged(notification);
                    OnPropertyChanged(new FullNameNotification(FullName));
                }
            }
        }

        protected virtual void OnPropertyChanged(AgeNotification notification)
        { }

        protected virtual void OnPropertyChanging(AgeNotification notification)
        { }

        protected virtual void OnPropertyChanged(FirstNameNotification notification)
        { }

        protected virtual void OnPropertyChanging(FirstNameNotification notification)
        { }

        protected virtual void OnPropertyChanged(LastNameNotification notification)
        { }

        protected virtual void OnPropertyChanging(LastNameNotification notification)
        { }

        protected virtual void OnPropertyChanged(FullNameNotification notification)
        { }

        protected virtual void OnPropertyChanging(FullNameNotification notification)
        { }

        public readonly struct AgeNotification : INotification
        {
            public readonly int Value;

            public AgeNotification(int value)
            {
                Value = value;
            }
        }

        public readonly struct FirstNameNotification : INotification
        {
            public readonly string Value;

            public FirstNameNotification(string value)
            {
                Value = value;
            }
        }

        public readonly struct LastNameNotification : INotification
        {
            public readonly string Value;

            public LastNameNotification(string value)
            {
                Value = value;
            }
        }

        public readonly struct FullNameNotification : INotification
        {
            public readonly string Value;

            public FullNameNotification(string value)
            {
                Value = value;
            }
        }

        [RelayCommand]
        private void GreetUser(MyViewModel model)
        {

        }

        private bool Validate() => false;

        [RelayCommand(CanExecute = nameof(Validate))]
        private void DoX(int x)
        {

        }
    }

    public partial class MyViewModelDataContext : IDataContext<MyViewModel>
    {
        public MyViewModel ViewModel { get; } = new();
    }
}