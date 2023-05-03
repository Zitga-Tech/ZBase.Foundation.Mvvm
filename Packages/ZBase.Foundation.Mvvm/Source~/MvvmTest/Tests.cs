using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ZBase.Foundation.Mvvm;
using ZBase.Foundation.Mvvm.Unions;

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
        private int _intField;

        [ObservableProperty]
        private TypeCode _typeCode;

        public int Index { get; }
    }

    public partial struct UnionTypCode : IUnion<TypeCode> { }
}
