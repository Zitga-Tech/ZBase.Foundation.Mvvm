using System;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = false, Inherited = false)]
    public sealed class AdapterAttribute : Attribute
    {
        public const int DEFAULT_ORDER = 1000;

        public Type FromType { get; }

        public Type ToType { get; }

        public int Order { get; }

        public AdapterAttribute(Type fromType, Type toType)
            : this(fromType, toType, DEFAULT_ORDER)
        { }

        public AdapterAttribute(Type fromType, Type toType, int order)
        {
            this.FromType = fromType;
            this.ToType = toType;
            this.Order = order;
        }
    }
}
