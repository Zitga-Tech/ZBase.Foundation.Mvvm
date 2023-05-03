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

    public interface IUnionTypeCode : IUnion<TypeCode> { }

    public partial struct UnionTypeCode2 : IUnion<TypeCode> { }

    public partial struct UnionTypeCode : IUnionTypeCode { }
}
