
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZBase.Foundation.Mvvm;
using ZBase.Foundation.Mvvm.Unions;

namespace X.TY.Z
{
    public partial class Ab
    {
        public partial class Name
        {
            public struct MyStr<T> { }
        }
    }
}

namespace MvvmTests
{
    public class Program
    {
        private PropertyChangeEventListener<Program> _listener;

        public static void Main()
        {
            var program = new Program();
            var model = new ModelA();

            program._listener = new PropertyChangeEventListener<Program>(program) {
                OnEventAction = (instance, args) => instance.Print(args.Value)
            };

            model.PropertyChanged(nameof(ModelA.IntValue), program._listener);

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

    public partial class ModelA : IObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Total))]
        private int _intValue;

        [ObservableProperty]
        private TypeCode _typeCode;

        [ObservableProperty]
        private X.TY.Z.Ab.Name.MyStr<MyEnum> _customValue;

        private int Total { get; }

        [RelayCommand]
        private void Process(X.TY.Z.Ab.Name.MyStr<MyEnum> a) { }
        
        private bool Validate(int x) => false;

        [RelayCommand(CanExecute = nameof(Validate))]
        private void DoX(int x)
        {

        }
    }

    public partial class ModelB : ModelA
    {
    }

    public sealed partial class ModelC : ModelB
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Total))]
        private float _floatValue;

        private int Total { get; }
    }

    public class ModelD { }

    public sealed partial class ModelE : ModelD, IObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Total))]
        private float _floatValue;

        private int Total { get; }
    }

    public partial class MyBinder : IBinder
    {
        public IDataContext DataContext { get; private set; }

        [Binding]
        private void OnUpdateAge(in Union value)
        {
        }
    }

    public partial class MyViewModel : IObservableObject
    {
        [ObservableProperty]
        private int _age;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        [NotifyCanExecuteChangedFor(nameof(GreetUserCommand))]
        private string _firstName;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullName))]
        [NotifyCanExecuteChangedFor(nameof(GreetUserCommand))]
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

    public abstract partial class MonoBinder : UnityEngine.MonoBehaviour, IBinder
    {
        public IDataContext DataContext { get; private set; }
    }

    public partial class UIBinder : MonoBinder
    {

    }

    [UnityEngine.RequireComponent(typeof(UnityEngine.CanvasGroup))]
    public partial class CanvasGroupBinder : UIBinder
    {
        private UnityEngine.CanvasGroup _canvasGroup;

        [Binding]
        private void OnUpdateAlpha(in Union value)
        {
            if (value.TryGetValue(out float alpha))
            {
                _canvasGroup.alpha = alpha;
            }
        }

        [Binding(Label = "interactable")]
        private void OnUpdateInteractable(in Union value)
        {
            if (value.TryGetValue(out bool interactable))
            {
                _canvasGroup.interactable = interactable;
            }
        }

        [Binding(Label = nameof(UnityEngine.CanvasGroup.blocksRaycasts))]
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
    public enum MyEnum { A, B, C }

    [StructLayout(LayoutKind.Explicit, Size = global::ZBase.Foundation.Mvvm.Unions.UnionData.SIZE)]
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

        [global::System.Diagnostics.CodeAnalysis.DoesNotReturn]
        private static void ValidateTypeId(in Union union)
        {
            if (union.TypeId != Union<MyEnum>.TypeId)
            {
                throw new InvalidCastException(
                    $"Cannot cast {union.TypeId.AsType()} to {Union<MyEnum>.TypeId.AsType()}"
                );
            }
        }

        public sealed class Converter : IUnionConverter<MyEnum>
        {
            public static readonly Converter Default = new Converter();

            [global::UnityEngine.Scripting.Preserve]
            static Converter()
            {
                Init();
            }

#if UNITY_5_3_OR_NEWER && UNITY_EDITOR
            [global::UnityEngine.RuntimeInitializeOnLoadMethod(global::UnityEngine.RuntimeInitializeLoadType.SubsystemRegistration)]
#endif
            private static void Init()
            {
                global::ZBase.Foundation.Mvvm.Unions.UnionConverter.TryRegister(Default);
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