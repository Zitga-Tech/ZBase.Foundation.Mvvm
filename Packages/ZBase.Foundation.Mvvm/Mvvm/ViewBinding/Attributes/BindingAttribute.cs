using System;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class BindingAttribute : Attribute
    {
        public string BindingLabel { get; }

        public string ConverterLabel { get; }

        public BindingAttribute() { }

        public BindingAttribute(string label)
        {
            BindingLabel = label;
            ConverterLabel = label;
        }

        public BindingAttribute(string bindingLabel, string converterLabel)
        {
            BindingLabel = bindingLabel;
            ConverterLabel = converterLabel;
        }
    }
}
