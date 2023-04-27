#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0090 // Use 'new(...)'
#pragma warning disable CA1822 // Mark members as static
#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE1006 // Naming Styles

using System;
using System.Runtime.CompilerServices;
using ZBase.Foundation.Mvvm;

namespace MvvmTests
{
    public class Program
    {
        private PropertyChangeEventListener<Program> _listener;

        public static void Main()
        {
            var program = new Program();
            var model = new MyViewModel();

            program._listener = new PropertyChangeEventListener<Program>(program) {
                OnEventAction = (instance, args) => instance.Print(args.Value)
            };

            model.PropertyChanged(MyViewModelPropertyNames.Age, program._listener);

            while (true)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.Spacebar:
                        model.Age += 1;
                        break;

                    default:
                        return;
                }
            }
        }

        private void Print(in ValueUnion value)
        {
            Console.WriteLine(value);
        }
    }

    public partial class TestViewModel : IObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Total))]
        private int _intValue;

        private int Total { get; }
    }

    public partial class MyViewModel : IObservableObject
    {
        //[ObservableProperty]
        private int _age;

        //[ObservableProperty]
        //[NotifyPropertyChangedFor(nameof(FullName))]
        //[NotifyCanExecuteChangedFor(nameof(GreetUserCommand))]
        private string _firstName;

        //[ObservableProperty]
        //[NotifyPropertyChangedFor(nameof(FullName))]
        //[NotifyCanExecuteChangedFor(nameof(GreetUserCommand))]
        private string _lastName;

        public string FullName => $"{_firstName} {_lastName}";

        //[RelayCommand]
        private void GreetUser(MyViewModel model)
        {

        }

        private bool Validate() => false;

        //[RelayCommand(CanExecute = nameof(Validate))]
        private void DoX(int x)
        {

        }
    }

    public partial class MyDataContext : UnityEngine.MonoBehaviour
    {
        [DataContextViewModel]
        private MyViewModel _viewModel;
    }

    public partial class BinderBase : UnityEngine.MonoBehaviour
    {
        protected IDataContext DataContext { get; private set; }
    }

    public partial class CanvasGroupBinder : BinderBase
    {
        private UnityEngine.CanvasGroup _canvasGroup;

        [BindingField]
        private void OnUpdateAlpha(in ValueUnion value)
        {
            if (value.TryGetValue(out float alpha))
            {
                _canvasGroup.alpha = alpha;
            }
        }

        [BindingField(Label = "interactable")]
        private void OnUpdateInteractable(in ValueUnion value)
        {
            if (value.TryGetValue(out bool interactable))
            {
                _canvasGroup.interactable = interactable;
            }
        }

        [BindingField(Label = nameof(UnityEngine.CanvasGroup.blocksRaycasts))]
        private void OnUpdateBlockRaycasts(in ValueUnion value)
        {
            if (value.TryGetValue(out bool blocksRaycasts))
            {
                _canvasGroup.blocksRaycasts = blocksRaycasts;
            }
        }
    }
}

namespace MvvmTests
{
    public static partial class MyViewModelPropertyNames
    {
        /// <inheritdoc cref="global::MvvmTests.MyViewModel.Age" />
        [global::System.CodeDom.Compiler.GeneratedCode("ZBase.Foundation.Mvvm.ObservablePropertyGenerator", "1.0.0")]
        public const string Age = nameof(Age);

        /// <inheritdoc cref="global::MvvmTests.MyViewModel.FirstName" />
        public const string FirstName = nameof(FirstName);

        /// <inheritdoc cref="global::MvvmTests.MyViewModel.LastName" />
        public const string LastName = nameof(LastName);

        /// <inheritdoc cref="global::MvvmTests.MyViewModel.FullName" />
        public const string FullName = nameof(FullName);
    }

    partial class MyViewModel : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler _ageChanging;
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler _firstNameChanging;
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler _lastNameChanging;

        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler _ageChanged;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler _firstNameChanged;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler _lastNameChanged;

        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler _fullNameChanged;

        /// <inheritdoc cref="global::MvvmTests.MyViewModel._age" />
        [global::System.CodeDom.Compiler.GeneratedCode("ZBase.Foundation.Mvvm.ObservablePropertyGenerator", "1.0.0")]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public int Age
        {
            get => _age;
            set
            {
                if (global::System.Collections.Generic.EqualityComparer<int>.Default.Equals(_age, value) == false)
                {
                    var args = new PropertyChangeEventArgs(this, MyViewModelPropertyNames.Age, value);

                    if (this._ageChanging != null)
                        this._ageChanging(args);

                    this._age = value;

                    OnAgeChanged(value);

                    if (this._ageChanged != null)
                        this._ageChanged(args);
                }
            }
        }

        /// <inheritdoc cref="global::MvvmTests.MyViewModel._firstName" />
        [global::System.CodeDom.Compiler.GeneratedCode("ZBase.Foundation.Mvvm.ObservablePropertyGenerator", "1.0.0")]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (global::System.Collections.Generic.EqualityComparer<string>.Default.Equals(_firstName, value) == false)
                {
                    OnFirstNameChanging(value);

                    if (this._firstNameChanging != null)
                        this._firstNameChanging(new PropertyChangeEventArgs(this, MyViewModelPropertyNames.FirstName, value));

                    this._firstName = value;

                    OnFirstNameChanged(value);

                    if (this._firstNameChanged != null)
                        this._firstNameChanged(new PropertyChangeEventArgs(this, MyViewModelPropertyNames.FirstName, value));

                    if (this._fullNameChanged != null)
                        this._fullNameChanged(new PropertyChangeEventArgs(this, MyViewModelPropertyNames.FullName, this.FullName));
                }
            }
        }

        /// <inheritdoc cref="global::MvvmTests.MyViewModel._lastName" />
        [global::System.CodeDom.Compiler.GeneratedCode("ZBase.Foundation.Mvvm.ObservablePropertyGenerator", "1.0.0")]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public string LastName
        {
            get => _lastName;
            set
            {
                if (global::System.Collections.Generic.EqualityComparer<string>.Default.Equals(_lastName, value) == false)
                {
                    OnLastNameChanging(value);

                    if (this._lastNameChanging != null)
                        this._lastNameChanging(new PropertyChangeEventArgs(this, MyViewModelPropertyNames.LastName, value));

                    this._lastName = value;

                    OnLastNameChanged(value);

                    if (this._lastNameChanged != null)
                        this._lastNameChanged(new PropertyChangeEventArgs(this, MyViewModelPropertyNames.LastName, value));

                    if (this._fullNameChanged != null)
                        this._fullNameChanged(new PropertyChangeEventArgs(this, MyViewModelPropertyNames.FullName, this.FullName));
                }
            }
        }

        partial void OnAgeChanging(int value);

        partial void OnAgeChanged(int value);

        partial void OnFirstNameChanging(string value);

        partial void OnFirstNameChanged(string value);

        partial void OnLastNameChanging(string value);

        partial void OnLastNameChanged(string value);

        /// <inheritdoc cref="global::ZBase.Foundation.Mvvm.INotifyPropertyChanging.PropertyChanging{TInstance}(string, PropertyChangeEventListener{TInstance})" />
        public void PropertyChanging<TInstance>(string propertyName, PropertyChangeEventListener<TInstance> listener)
            where TInstance : class
        {
            if (propertyName == null)
                throw new global::System.ArgumentNullException(nameof(propertyName));

            if (listener == null)
                throw new global::System.ArgumentNullException(nameof(listener));

            switch (propertyName)
            {
                case MyViewModelPropertyNames.Age:
                {
                    if (listener is PropertyChangeEventListener<TInstance> ageValueListener)
                    {
                        _ageChanging += ageValueListener.OnEvent;
                        ageValueListener.OnDetachAction = (listener) => _ageChanging -= listener.OnEvent;
                        return;
                    }

                    break;
                }

                case MyViewModelPropertyNames.FirstName:
                {
                    if (listener is PropertyChangeEventListener<TInstance> firstNameValueListener)
                    {
                        _firstNameChanging += firstNameValueListener.OnEvent;
                        firstNameValueListener.OnDetachAction = (listener) => _firstNameChanging -= listener.OnEvent;
                        return;
                    }

                    break;
                }

                case MyViewModelPropertyNames.LastName:
                {
                    if (listener is PropertyChangeEventListener<TInstance> lastNameValueListener)
                    {
                        _lastNameChanging += lastNameValueListener.OnEvent;
                        lastNameValueListener.OnDetachAction = (listener) => _lastNameChanging -= listener.OnEvent;
                        return;
                    }

                    break;
                }
            }
        }

        /// <inheritdoc cref="global::ZBase.Foundation.Mvvm.INotifyPropertyChanged.PropertyChanged{TInstance}(string, PropertyChangeEventListener{TInstance})" />
        public void PropertyChanged<TInstance>(string propertyName, PropertyChangeEventListener<TInstance> listener)
           where TInstance : class
        {
            if (propertyName == null)
                throw new global::System.ArgumentNullException(nameof(propertyName));

            if (listener == null)
                throw new global::System.ArgumentNullException(nameof(listener));

            switch (propertyName)
            {
                case MyViewModelPropertyNames.Age:
                {
                    _ageChanged += listener.OnEvent;
                    listener.OnDetachAction = (listener) => _ageChanged -= listener.OnEvent;
                    break;
                }

                case MyViewModelPropertyNames.FirstName:
                {
                    _firstNameChanged += listener.OnEvent;
                    listener.OnDetachAction = (listener) => _firstNameChanged -= listener.OnEvent;
                    break;
                }

                case MyViewModelPropertyNames.LastName:
                {
                    _lastNameChanged += listener.OnEvent;
                    listener.OnDetachAction = (listener) => _lastNameChanged -= listener.OnEvent;
                    break;
                }

                case MyViewModelPropertyNames.FullName:
                {
                    _fullNameChanged += listener.OnEvent;
                    listener.OnDetachAction = (listener) => _fullNameChanged -= listener.OnEvent;
                    break;
                }
            }
        }
    }
}

namespace MvvmTests
{
    partial class MyDataContext : global::ZBase.Foundation.Mvvm.IDataContext
    {
        public IObservableObject ViewModel
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _viewModel;
        }
    }
}

namespace MvvmTests
{
    partial class CanvasGroupBinder
    {
        [global::UnityEngine.SerializeField]
        private BindingField _fieldOnUpdateAlpha = new BindingField() { Label = nameof(OnUpdateAlpha) };

        [global::UnityEngine.SerializeField]
        private BindingField _fieldOnUpdateInteractable = new BindingField() { Label = "interactable" };

        [global::UnityEngine.SerializeField]
        private BindingField _fieldOnUpdateBlockRaycasts = new BindingField() { Label = "blocksRaycasts" };

        [global::UnityEngine.SerializeReference]
        private Converter _converterOnUpdateAlpha = new Converter() { Label = nameof(OnUpdateAlpha) };

        [global::UnityEngine.SerializeReference]
        private Converter _converterOnUpdateInteractable = new Converter() { Label = "interactable" };

        [global::UnityEngine.SerializeReference]
        private Converter _converterOnUpdateBlockRaycasts = new Converter() { Label = "blocksRaycasts" };

        private PropertyChangeEventListener<CanvasGroupBinder> _listenerOnUpdateAlpha;
        private PropertyChangeEventListener<CanvasGroupBinder> _listenerOnUpdateInteractable;
        private PropertyChangeEventListener<CanvasGroupBinder> _listenerOnUpdateBlockRaycasts;

        //protected override void Initialize()
        protected virtual void Initialize()
        {
            if (DataContext.ViewModel is not INotifyPropertyChanged inpc)
            {
                return;
            }

            _listenerOnUpdateAlpha = new PropertyChangeEventListener<CanvasGroupBinder>(this) {
                OnEventAction = (instance, args) => instance.OnUpdateAlpha(_converterOnUpdateAlpha.Convert(args.Value))
            };

            _listenerOnUpdateInteractable = new PropertyChangeEventListener<CanvasGroupBinder>(this) {
                OnEventAction = (instance, args) => instance.OnUpdateInteractable(_converterOnUpdateInteractable.Convert(args.Value))
            };

            _listenerOnUpdateBlockRaycasts = new PropertyChangeEventListener<CanvasGroupBinder>(this) {
                OnEventAction = (instance, args) => instance.OnUpdateBlockRaycasts(_converterOnUpdateBlockRaycasts.Convert(args.Value))
            };

            inpc.PropertyChanged(_fieldOnUpdateAlpha.Member, _listenerOnUpdateAlpha);
            inpc.PropertyChanged(_fieldOnUpdateInteractable.Member, _listenerOnUpdateInteractable);
            inpc.PropertyChanged(_fieldOnUpdateBlockRaycasts.Member, _listenerOnUpdateBlockRaycasts);
        }
    }
}