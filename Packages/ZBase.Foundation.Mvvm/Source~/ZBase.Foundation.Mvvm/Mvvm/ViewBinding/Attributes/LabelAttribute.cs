using System;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    /// <summary>
    /// Provides label text that will be collected to show on the Unity Inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class LabelAttribute : Attribute
    {
        public string Value { get; }

        public LabelAttribute(string value)
        {
            this.Value = value;
        }
    }
}
