
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZBase.Foundation.Mvvm;
using ZBase.Foundation.Unions;

namespace X.TY.Z
{
    public struct MyStr<T> { }
}

namespace MvvmTests
{
    public class Program
    {
        private PropertyChangeEventListener<Program> _listener;

        public static void Main()
        {
            var program = new Program();
            var model = new TestViewModel();

            program._listener = new PropertyChangeEventListener<Program>(program) {
                OnEventAction = (instance, args) => instance.Print(args.Value)
            };

            model.PropertyChanged(nameof(TestViewModel.IntValue), program._listener);

            while (true)
            {
                var key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.Spacebar:
                        model.IntValue += 1;
                        break;

                    default:
                        return;
                }
            }
        }

        private void Print(in Union value)
        {
            Console.WriteLine(value);
        }
    }

    public partial class TestViewModel : IObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Total))]
        private int _intValue;

        [ObservableProperty]
        private TypeCode _typeCode;

        [ObservableProperty]
        private X.TY.Z.MyStr<MyEnum[]> _customValue;

        private int Total { get; }
        
        private bool Validate(int x) => false;

        [RelayCommand(CanExecute = nameof(Validate))]
        private void DoX(int x)
        {

        }
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

    [UnityEngine.RequireComponent(typeof(UnityEngine.CanvasGroup))]
    public partial class CanvasGroupBinder : BinderBase
    {
        private UnityEngine.CanvasGroup _canvasGroup;

        [BindingField]
        private void OnUpdateAlpha(in Union value)
        {
            if (value.TryGetValue(out float alpha))
            {
                _canvasGroup.alpha = alpha;
            }
        }

        [BindingField(Label = "interactable")]
        private void OnUpdateInteractable(in Union value)
        {
            if (value.TryGetValue(out bool interactable))
            {
                _canvasGroup.interactable = interactable;
            }
        }

        [BindingField(Label = nameof(UnityEngine.CanvasGroup.blocksRaycasts))]
        private void OnUpdateBlockRaycasts(in Union value)
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
    partial class MyViewModel : INotifyPropertyChanging, INotifyPropertyChanged
    {
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler _onChangingAge;
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler _onChangingFirstName;
        private event global::ZBase.Foundation.Mvvm.PropertyChangingEventHandler _onChangingLastName;

        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler _onChangedAge;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler _onChangedFirstName;
        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler _onChangedLastName;

        private event global::ZBase.Foundation.Mvvm.PropertyChangedEventHandler _onChangedFullName;

        private IUnionConverter<int> _unionConverterInt;

        private IUnionConverter<int> UnionConverterInt
        {
            get => _unionConverterInt ??= UnionConverter.GetConverter<int>();
        }

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
                    OnAgeChanging(value);
                    var args = new PropertyChangeEventArgs(this, nameof(this.Age), UnionConverterInt.ToUnion(value));
                    this._onChangingAge?.Invoke(args);
                    this._age = value;
                    OnAgeChanged(value);
                    this._onChangedAge?.Invoke(args);
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
                    var args = new PropertyChangeEventArgs(this, nameof(this.Age), value);
                    this._onChangingFirstName?.Invoke(args);
                    this._firstName = value;
                    OnFirstNameChanged(value);
                    this._onChangedFirstName?.Invoke(args);
                    this._onChangedFullName?.Invoke(new PropertyChangeEventArgs(this, nameof(this.FullName), this.FullName));
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
                    var args = new PropertyChangeEventArgs(this, nameof(this.LastName), value);
                    this._onChangingLastName?.Invoke(args);
                    this._lastName = value;
                    OnLastNameChanged(value);
                    this._onChangedLastName?.Invoke(args);
                    this._onChangedFullName?.Invoke(new PropertyChangeEventArgs(this, nameof(this.FullName), this.FullName));
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
        public virtual void PropertyChanging<TInstance>(string propertyName, global::ZBase.Foundation.Mvvm.PropertyChangeEventListener<TInstance> listener)
            where TInstance : class
        {
            if (propertyName == null)
                throw new global::System.ArgumentNullException(nameof(propertyName));

            if (listener == null)
                throw new global::System.ArgumentNullException(nameof(listener));

            switch (propertyName)
            {
                case nameof(this.Age):
                {
                    _onChangingAge += listener.OnEvent;
                    listener.OnDetachAction = (listener) => _onChangingAge -= listener.OnEvent;
                    break;
                }

                case nameof(this.FirstName):
                {
                    _onChangingFirstName += listener.OnEvent;
                    listener.OnDetachAction = (listener) => _onChangingFirstName -= listener.OnEvent;
                    break;
                }

                case nameof(this.LastName):
                {
                    _onChangingLastName += listener.OnEvent;
                    listener.OnDetachAction = (listener) => _onChangingLastName -= listener.OnEvent;
                    break;
                }
            }
        }

        /// <inheritdoc cref="global::ZBase.Foundation.Mvvm.INotifyPropertyChanged.PropertyChanged{TInstance}(string, PropertyChangeEventListener{TInstance})" />
        public virtual void PropertyChanged<TInstance>(string propertyName, PropertyChangeEventListener<TInstance> listener)
           where TInstance : class
        {
            if (propertyName == null)
                throw new global::System.ArgumentNullException(nameof(propertyName));

            if (listener == null)
                throw new global::System.ArgumentNullException(nameof(listener));
            
            switch (propertyName)
            {
                case nameof(this.Age):
                {
                    _onChangedAge += listener.OnEvent;
                    listener.OnDetachAction = (listener) => _onChangedAge -= listener.OnEvent;
                    break;
                }

                case nameof(this.FirstName):
                {
                    _onChangedFirstName += listener.OnEvent;
                    listener.OnDetachAction = (listener) => _onChangedFirstName -= listener.OnEvent;
                    break;
                }

                case nameof(this.LastName):
                {
                    _onChangedLastName += listener.OnEvent;
                    listener.OnDetachAction = (listener) => _onChangedLastName -= listener.OnEvent;
                    break;
                }

                case nameof(this.FullName):
                {
                    _onChangedFullName += listener.OnEvent;
                    listener.OnDetachAction = (listener) => _onChangedFullName -= listener.OnEvent;
                    break;
                }
                
                default: throw new InvalidOperationException("Property name is invalid");
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

namespace MvvmTests
{
    public enum MyEnum { A, B, C }

    [StructLayout(LayoutKind.Explicit)]
    public readonly partial struct UnionMyEnum : IUnion<MyEnum>
    {
        [FieldOffset(UnionBase.META_OFFSET)] public readonly Union<MyEnum> Union;
        [FieldOffset(UnionBase.DATA_OFFSET)] public readonly MyEnum Value;

        public UnionMyEnum(MyEnum value)
        {
            Union = new Union(UnionTypeKind.ValueType, Union<MyEnum>.TypeId);
            Value = value;
        }

        public UnionMyEnum(in Union<MyEnum> union) : this()
        {
            Union = union;
        }

        public UnionMyEnum(in Union union) : this()
        {
            ValidateTypeId(union);
            Union = union;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UnionMyEnum(MyEnum value)
            => new UnionMyEnum(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Union(in UnionMyEnum value)
            => value.Union;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Union<MyEnum>(in UnionMyEnum value)
            => value.Union;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator UnionMyEnum(in Union<MyEnum> value)
            => new UnionMyEnum(value);

        [DoesNotReturn]
        private static void ValidateTypeId(in Union union)
        {
            throw new InvalidCastException(
                $"Cannot cast {union.TypeId.AsType()} to {Union<MyEnum>.TypeId.AsType()}"
            );
        }

        public sealed class Converter : IUnionConverter<MyEnum>
        {
            public static readonly Converter Default = new Converter();

            static Converter()
            {
#if !UNITY_5_3_OR_NEWER || !UNITY_EDITOR
                Init();
#endif
            }

#if UNITY_5_3_OR_NEWER && UNITY_EDITOR
            [UnityEngine.RuntimeInitializeOnLoadMethod(UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
            private static void Init()
            {
                ZBase.Foundation.Unions.UnionConverter.TryRegister(Default);
            }

            private Converter() { }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Union ToUnion(MyEnum value)
                => new UnionMyEnum(value);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Union<MyEnum> ToUnionT(MyEnum value)
                => new UnionMyEnum(value).Union;

            public bool TryGetValue(in Union union, out MyEnum result)
            {
                if (union.TypeId == Union<MyEnum>.TypeId)
                {
                    var unionMyEnum = new UnionMyEnum(union);
                    result = unionMyEnum.Value;
                    return true;
                }

                result = default;
                return false;
            }

            public bool TrySetValue(in Union union, ref MyEnum result)
            {
                if (union.TypeId == Union<MyEnum>.TypeId)
                {
                    var unionMyEnum = new UnionMyEnum(union);
                    result = unionMyEnum.Value;
                    return true;
                }

                return false;
            }
        }
    }
}