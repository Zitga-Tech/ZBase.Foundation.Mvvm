using System;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class AdapterAttribute : Attribute
    {
        public Type FromType { get; }

        public Type ToType { get; }

        public AdapterAttribute(Type fromType, Type toType)
        {
            this.FromType = fromType;
            this.ToType = toType;
        }
    }
}
