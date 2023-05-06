using System;

namespace ZBase.Foundation.Mvvm
{
    /// <summary>
    /// Provides label text that will be shown on the Unity Inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public sealed class LabelAttribute : Attribute
    {
        public string Value { get; }

        public LabelAttribute(string value)
        {
            this.Value = value;
        }
    }
}
