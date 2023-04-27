using System;

namespace ZBase.Foundation.Mvvm
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class DataContextViewModelAttribute : Attribute { }
}
