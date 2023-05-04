using System;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    /// <summary>
    /// An attribute that indicates that a given method can be binded to any observable property.
    /// </summary>
    /// <remarks>
    /// This attribute is not intended to be used directly by user code to decorate user-defined types.
    /// <br/>
    /// However, it can be used in other contexts, such as reflection.
    /// </remarks>
    /// <seealso cref="BindingAttribute"/>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class BindingMethodInfoAttribute : Attribute
    {
        public string MethodName { get; }

        public Type ParameterType { get; }

        public BindingMethodInfoAttribute(string methodName, Type parameterType)
        {
            this.MethodName = methodName;
            this.ParameterType = parameterType;
        }
    }
}
