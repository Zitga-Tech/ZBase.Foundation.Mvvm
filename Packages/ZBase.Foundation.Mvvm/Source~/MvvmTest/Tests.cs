#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable IDE0060 // Remove unused parameter
#pragma warning disable IDE0090 // Use 'new(...)'

using System.Runtime.CompilerServices;
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
        private void OnUpdateAlpha(float value)
        {
            _canvasGroup.alpha = value;
        }

        [BindingField(Label = "interactable")]
        private void OnUpdateInteractable(bool value)
        {
            _canvasGroup.interactable = value;
        }

        [BindingField(Label = nameof(UnityEngine.CanvasGroup.blocksRaycasts))]
        private void OnUpdateBlockRaycasts(bool value)
        {
            _canvasGroup.blocksRaycasts = value;
        }

        [BindingFieldFallback(nameof(OnUpdateAlpha))]
        private void OnUpdateAlpha(object value)
        {

        }

        [BindingFieldFallback(nameof(OnUpdateInteractable))]
        private void OnUpdateInteractable(object value)
        {

        }

        [BindingFieldFallback(nameof(OnUpdateBlockRaycasts))]
        private void OnUpdateBlockRaycasts(object value)
        {

        }
    }
}

namespace MvvmTests
{
    partial class MyViewModel : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler<AgeChangeEventArgs> _ageChanging;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<AgeChangeEventArgs> _ageChanged;
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler<FirstNameChangeEventArgs> _firstNameChanging;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<FirstNameChangeEventArgs> _firstNameChanged;
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler<LastNameChangeEventArgs> _lastNameChanging;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<LastNameChangeEventArgs> _lastNameChanged;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<FullNameChangeEventArgs> _fullNameChanged;

        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler<PropertyChangeEventArgs<int>> _ageValueChanging;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<PropertyChangeEventArgs<int>> _ageValueChanged;
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler<PropertyChangeEventArgs<string>> _firstNameValueChanging;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<PropertyChangeEventArgs<string>> _firstNameValueChanged;
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler<PropertyChangeEventArgs<string>> _lastNameValueChanging;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<PropertyChangeEventArgs<string>> _lastNameValueChanged;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<PropertyChangeEventArgs<string>> _fullNameValueChanged;

        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler<PropertyChangeEventArgs<object>> _ageObjectChanging;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<PropertyChangeEventArgs<object>> _ageObjectChanged;
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler<PropertyChangeEventArgs<object>> _firstNameObjectChanging;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<PropertyChangeEventArgs<object>> _firstNameObjectChanged;
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler<PropertyChangeEventArgs<object>> _lastNameObjectChanging;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<PropertyChangeEventArgs<object>> _lastNameObjectChanged;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<PropertyChangeEventArgs<object>> _fullNameObjectChanged;

        [global::System.CodeDom.Compiler.GeneratedCode("ZBase.Foundation.Mvvm.ObservablePropertyGenerator", "1.0.0")]
        [global::System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
        public int Age
        {
            get => _age;
            set
            {
                if (global::System.Collections.Generic.EqualityComparer<int>.Default.Equals(_age, value) == false)
                {
                    var ageChangeEventArgs = new AgeChangeEventArgs(this, value);

                    OnChanging(ageChangeEventArgs);
                    OnPropertyChanging(this._ageChanging, ageChangeEventArgs);
                    OnPropertyChanging(this._ageValueChanging, ageChangeEventArgs);
                    OnPropertyChanging(this._ageObjectChanging, ageChangeEventArgs);

                    this._age = value;

                    OnChanged(ageChangeEventArgs);
                    OnPropertyChanged(this._ageChanged, ageChangeEventArgs);
                    OnPropertyChanged(this._ageValueChanged, ageChangeEventArgs);
                    OnPropertyChanged(this._ageObjectChanged, ageChangeEventArgs);
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
                    var firstNameChangeEventArgs = new FirstNameChangeEventArgs(this, value);

                    OnChanging(firstNameChangeEventArgs);
                    OnPropertyChanging(this._firstNameChanging, firstNameChangeEventArgs);
                    OnPropertyChanging(this._firstNameValueChanging, firstNameChangeEventArgs);
                    OnPropertyChanging(this._firstNameObjectChanging, firstNameChangeEventArgs);

                    this._firstName = value;

                    OnChanged(firstNameChangeEventArgs);
                    OnPropertyChanged(this._firstNameChanged, firstNameChangeEventArgs);
                    OnPropertyChanged(this._firstNameValueChanged, firstNameChangeEventArgs);
                    OnPropertyChanged(this._firstNameObjectChanged, firstNameChangeEventArgs);

                    var fullNameChangeEventArgs = new FullNameChangeEventArgs(this, this.FullName);
                    OnPropertyChanged(this._fullNameChanged, fullNameChangeEventArgs);
                    OnPropertyChanged(this._fullNameValueChanged, fullNameChangeEventArgs);
                    OnPropertyChanged(this._fullNameObjectChanged, fullNameChangeEventArgs);
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
                    var lastNameChangeEventArgs = new LastNameChangeEventArgs(this, value);
                    var lastNameValueChangeEventArgs = (PropertyChangeEventArgs<string>)lastNameChangeEventArgs;

                    OnChanging(lastNameChangeEventArgs);
                    OnPropertyChanging(this._lastNameChanging, lastNameChangeEventArgs);
                    OnPropertyChanging(this._lastNameValueChanging, lastNameChangeEventArgs);
                    OnPropertyChanging(this._lastNameObjectChanging, lastNameChangeEventArgs);

                    this._lastName = value;

                    OnChanged(lastNameChangeEventArgs);
                    OnPropertyChanged(this._lastNameChanged, lastNameChangeEventArgs);
                    OnPropertyChanged(this._lastNameValueChanged, lastNameChangeEventArgs);
                    OnPropertyChanged(this._lastNameObjectChanged, lastNameChangeEventArgs);

                    var fullNameChangeEventArgs = new FullNameChangeEventArgs(this, this.FullName);
                    OnPropertyChanged(this._fullNameChanged, fullNameChangeEventArgs);
                    OnPropertyChanged(this._fullNameValueChanged, fullNameChangeEventArgs);
                    OnPropertyChanged(this._fullNameObjectChanged, fullNameChangeEventArgs);
                }
            }
        }

        partial void OnChanging(AgeChangeEventArgs args);

        partial void OnChanged(AgeChangeEventArgs args);

        partial void OnChanging(FirstNameChangeEventArgs args);

        partial void OnChanged(FirstNameChangeEventArgs args);

        partial void OnChanging(LastNameChangeEventArgs args);

        partial void OnChanged(LastNameChangeEventArgs args);

        public void PropertyChanging<TInstance, TEventArgs>(string propertyName, PropertyChangeEventListener<TInstance, TEventArgs> listener)
            where TInstance : class
            where TEventArgs : IPropertyChangeEventArgs
        {
            if (propertyName == null)
                throw new global::System.ArgumentNullException(nameof(propertyName));

            if (listener == null)
                throw new global::System.ArgumentNullException(nameof(listener));

            switch (propertyName)
            {
                case PropertyNames.Age:
                {
                    if (listener is PropertyChangeEventListener<TInstance, AgeChangeEventArgs> ageListener)
                    {
                        _ageChanging += ageListener.OnEvent;
                        ageListener.OnDetachAction = (listener) => _ageChanging -= listener.OnEvent;
                        return;
                    }

                    if (listener is PropertyChangeEventListener<TInstance, PropertyChangeEventArgs<int>> ageValueListener)
                    {
                        _ageValueChanging += ageValueListener.OnEvent;
                        ageValueListener.OnDetachAction = (listener) => _ageValueChanging -= listener.OnEvent;
                        return;
                    }

                    break;
                }

                case PropertyNames.FirstName:
                {
                    if (listener is PropertyChangeEventListener<TInstance, FirstNameChangeEventArgs> firstNameListener)
                    {
                        _firstNameChanging += firstNameListener.OnEvent;
                        firstNameListener.OnDetachAction = (listener) => _firstNameChanging -= listener.OnEvent;
                        return;
                    }

                    if (listener is PropertyChangeEventListener<TInstance, PropertyChangeEventArgs<string>> firstNameValueListener)
                    {
                        _firstNameValueChanging += firstNameValueListener.OnEvent;
                        firstNameValueListener.OnDetachAction = (listener) => _firstNameValueChanging -= listener.OnEvent;
                        return;
                    }

                    break;
                }

                case PropertyNames.LastName:
                {
                    if (listener is PropertyChangeEventListener<TInstance, LastNameChangeEventArgs> lastNameListener)
                    {
                        _lastNameChanging += lastNameListener.OnEvent;
                        lastNameListener.OnDetachAction = (listener) => _lastNameChanging -= listener.OnEvent;
                        return;
                    }

                    if (listener is PropertyChangeEventListener<TInstance, PropertyChangeEventArgs<string>> lastNameValueListener)
                    {
                        _lastNameValueChanging += lastNameValueListener.OnEvent;
                        lastNameValueListener.OnDetachAction = (listener) => _lastNameValueChanging -= listener.OnEvent;
                        return;
                    }

                    break;
                }
            }
        }

        public void PropertyChanged<TInstance, TEventArgs>(string propertyName, PropertyChangeEventListener<TInstance, TEventArgs> listener)
           where TInstance : class
           where TEventArgs : IPropertyChangeEventArgs
        {
            if (propertyName == null)
                throw new global::System.ArgumentNullException(nameof(propertyName));

            if (listener == null)
                throw new global::System.ArgumentNullException(nameof(listener));

            switch (propertyName)
            {
                case PropertyNames.Age:
                {
                    if (listener is PropertyChangeEventListener<TInstance, AgeChangeEventArgs> ageListener)
                    {
                        _ageChanged += ageListener.OnEvent;
                        ageListener.OnDetachAction = (listener) => _ageChanged -= listener.OnEvent;
                        return;
                    }

                    if (listener is PropertyChangeEventListener<TInstance, PropertyChangeEventArgs<int>> ageValueListener)
                    {
                        _ageValueChanged += ageValueListener.OnEvent;
                        ageValueListener.OnDetachAction = (listener) => _ageValueChanged -= listener.OnEvent;
                        return;
                    }

                    break;
                }

                case PropertyNames.FirstName:
                {
                    if (listener is PropertyChangeEventListener<TInstance, FirstNameChangeEventArgs> firstNameListener)
                    {
                        _firstNameChanged += firstNameListener.OnEvent;
                        firstNameListener.OnDetachAction = (listener) => _firstNameChanged -= listener.OnEvent;
                        return;
                    }

                    if (listener is PropertyChangeEventListener<TInstance, PropertyChangeEventArgs<string>> firstNameValueListener)
                    {
                        _firstNameValueChanged += firstNameValueListener.OnEvent;
                        firstNameValueListener.OnDetachAction = (listener) => _firstNameValueChanged -= listener.OnEvent;
                        return;
                    }

                    break;
                }

                case PropertyNames.LastName:
                {
                    if (listener is PropertyChangeEventListener<TInstance, LastNameChangeEventArgs> lastNameListener)
                    {
                        _lastNameChanged += lastNameListener.OnEvent;
                        lastNameListener.OnDetachAction = (listener) => _lastNameChanged -= listener.OnEvent;
                        return;
                    }

                    if (listener is PropertyChangeEventListener<TInstance, PropertyChangeEventArgs<string>> lastNameValueListener)
                    {
                        _lastNameValueChanged += lastNameValueListener.OnEvent;
                        lastNameValueListener.OnDetachAction = (listener) => _lastNameValueChanged -= listener.OnEvent;
                        return;
                    }

                    break;
                }

                case PropertyNames.FullName:
                {
                    if (listener is PropertyChangeEventListener<TInstance, FullNameChangeEventArgs> fullNameListener)
                    {
                        _fullNameChanged += fullNameListener.OnEvent;
                        fullNameListener.OnDetachAction = (listener) => _fullNameChanged -= listener.OnEvent;
                        return;
                    }

                    if (listener is PropertyChangeEventListener<TInstance, PropertyChangeEventArgs<string>> fullNameValueListener)
                    {
                        _fullNameValueChanged += fullNameValueListener.OnEvent;
                        fullNameValueListener.OnDetachAction = (listener) => _fullNameValueChanged -= listener.OnEvent;
                        return;
                    }

                    break;
                }
            }
        }

        protected virtual void OnPropertyChanging<TEventArgs>(global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler<TEventArgs> e, TEventArgs args)
            where TEventArgs : struct, global::ZBase.Foundation.Mvvm.IPropertyChangeEventArgs
        {
            e?.Invoke(args);
        }

        protected virtual void OnPropertyChanged<TEventArgs>(global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler<TEventArgs> e, TEventArgs args)
            where TEventArgs : struct, global::ZBase.Foundation.Mvvm.IPropertyChangeEventArgs
        {
            e?.Invoke(args);
        }

        private static class PropertyNames
        {
#pragma warning disable IDE1006 // Naming Styles
            public const string Age = nameof(global::MvvmTests.MyViewModel.Age);
            public const string FirstName = nameof(global::MvvmTests.MyViewModel.FirstName);
            public const string LastName = nameof(global::MvvmTests.MyViewModel.LastName);
            public const string FullName = nameof(global::MvvmTests.MyViewModel.FullName);
#pragma warning restore IDE1006 // Naming Styles
        }

        public readonly struct AgeChangeEventArgs : global::ZBase.Foundation.Mvvm.IPropertyChangeEventArgs
        {
            public readonly MyViewModel Sender;
            public readonly int Value;

            public string PropertyName
            {
                [global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]
                get => PropertyNames.Age;
            }

            public AgeChangeEventArgs(MyViewModel sender, int value)
            {
                Sender = sender;
                Value = value;
            }

            public static implicit operator PropertyChangeEventArgs<int>(AgeChangeEventArgs args)
                => new PropertyChangeEventArgs<int>(args.Sender, args.PropertyName, args.Value);

            public static implicit operator PropertyChangeEventArgs<object>(AgeChangeEventArgs args)
                => new PropertyChangeEventArgs<object>(args.Sender, args.PropertyName, args.Value);
        }

        public readonly struct FirstNameChangeEventArgs : IPropertyChangeEventArgs
        {
            public readonly MyViewModel Sender;
            public readonly string Value;

            public string PropertyName
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => PropertyNames.FirstName;
            }

            public FirstNameChangeEventArgs(MyViewModel sender, string value)
            {
                Sender = sender;
                Value = value;
            }

            public static implicit operator PropertyChangeEventArgs<string>(FirstNameChangeEventArgs args)
                => new PropertyChangeEventArgs<string>(args.Sender, args.PropertyName, args.Value);

            public static implicit operator PropertyChangeEventArgs<object>(FirstNameChangeEventArgs args)
                => new PropertyChangeEventArgs<object>(args.Sender, args.PropertyName, args.Value);
        }

        public readonly struct LastNameChangeEventArgs : IPropertyChangeEventArgs
        {
            public readonly MyViewModel Sender;
            public readonly string Value;

            public string PropertyName
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => PropertyNames.LastName;
            }

            public LastNameChangeEventArgs(MyViewModel sender, string value)
            {
                Sender = sender;
                Value = value;
            }

            public static implicit operator PropertyChangeEventArgs<string>(LastNameChangeEventArgs args)
                => new PropertyChangeEventArgs<string>(args.Sender, args.PropertyName, args.Value);

            public static implicit operator PropertyChangeEventArgs<object>(LastNameChangeEventArgs args)
                => new PropertyChangeEventArgs<object>(args.Sender, args.PropertyName, args.Value);
        }

        public readonly struct FullNameChangeEventArgs : IPropertyChangeEventArgs
        {
            public readonly MyViewModel Sender;
            public readonly string Value;

            public string PropertyName
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => PropertyNames.FullName;
            }

            public FullNameChangeEventArgs(MyViewModel sender, string value)
            {
                Sender = sender;
                Value = value;
            }

            public static implicit operator PropertyChangeEventArgs<string>(FullNameChangeEventArgs args)
                => new PropertyChangeEventArgs<string>(args.Sender, args.PropertyName, args.Value);

            public static implicit operator PropertyChangeEventArgs<object>(FullNameChangeEventArgs args)
                => new PropertyChangeEventArgs<object>(args.Sender, args.PropertyName, args.Value);
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

        private PropertyChangeEventListener<CanvasGroupBinder, PropertyChangeEventArgs<float>> _listenerOnUpdateAlpha;
        private PropertyChangeEventListener<CanvasGroupBinder, PropertyChangeEventArgs<bool>> _listenerOnUpdateInteractable;
        private PropertyChangeEventListener<CanvasGroupBinder, PropertyChangeEventArgs<bool>> _listenerOnUpdateBlockRaycasts;

        //protected override void Initialize()
        protected virtual void Initialize()
        {
            if (DataContext.ViewModel is not INotifyPropertyChanged inpc)
            {
                return;
            }

            _listenerOnUpdateAlpha = new PropertyChangeEventListener<CanvasGroupBinder, PropertyChangeEventArgs<float>>(this) {
                OnEventAction = (instance, args) => instance.OnUpdateAlpha(args.Value)
            };

            _listenerOnUpdateInteractable = new PropertyChangeEventListener<CanvasGroupBinder, PropertyChangeEventArgs<bool>>(this) {
                OnEventAction = (instance, args) => instance.OnUpdateInteractable(args.Value)
            };

            _listenerOnUpdateBlockRaycasts = new PropertyChangeEventListener<CanvasGroupBinder, PropertyChangeEventArgs<bool>>(this) {
                OnEventAction = (instance, args) => instance.OnUpdateBlockRaycasts(args.Value)
            };

            inpc.PropertyChanged(_fieldOnUpdateAlpha.Member, _listenerOnUpdateAlpha);
            inpc.PropertyChanged(_fieldOnUpdateInteractable.Member, _listenerOnUpdateInteractable);
            inpc.PropertyChanged(_fieldOnUpdateBlockRaycasts.Member, _listenerOnUpdateBlockRaycasts);
        }
    }
}