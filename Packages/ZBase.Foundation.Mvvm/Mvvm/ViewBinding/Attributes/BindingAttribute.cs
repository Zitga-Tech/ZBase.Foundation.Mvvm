using System;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class BindingAttribute : Attribute
    {
        public string Label { get; set; }
    }
}
