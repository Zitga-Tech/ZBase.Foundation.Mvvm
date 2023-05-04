using System;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public sealed class BindingAttribute : Attribute
    {
        public string BindingFieldLabel { get; }

        public string ConverterLabel { get; }

        public BindingAttribute() { }

        public BindingAttribute(string label)
        {
            BindingFieldLabel = label;
            ConverterLabel = label;
        }

        public BindingAttribute(string bindingFieldLabel, string converterLabel)
        {
            BindingFieldLabel = bindingFieldLabel;
            ConverterLabel = converterLabel;
        }
    }
}
