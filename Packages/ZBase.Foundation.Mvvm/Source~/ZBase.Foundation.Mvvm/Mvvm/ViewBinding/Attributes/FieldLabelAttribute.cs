using System;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    /// <summary>
    /// Provides label text metadata for <see cref="BindingField"/> and <see cref="Converter"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class FieldLabelAttribute : Attribute
    {
        public string Value { get; }

        public FieldLabelAttribute(string value)
        {
            this.Value = value;
        }
    }
}
