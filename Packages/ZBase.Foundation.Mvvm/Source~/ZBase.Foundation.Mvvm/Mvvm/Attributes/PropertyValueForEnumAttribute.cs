using System;

namespace ZBase.Foundation.Mvvm
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false, Inherited = false)]
    public sealed class PropertyValueForEnumAttribute : Attribute { }
}
