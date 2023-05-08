﻿using System;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    /// <summary>
    /// An attribute that indicates that a given property is generated by the Binder generator.
    /// </summary>
    /// <remarks>
    /// This attribute is not intended to be used directly by user code to decorate user-defined properties.
    /// <br/>
    /// However, it can be used in other contexts, such as reflection.
    /// </remarks>
    /// <seealso cref="BindingProperty"/>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class GeneratedBindingPropertyAttribute : Attribute
    {
        public string ForMemberName { get; }

        public Type ForMemberType { get; }

        public GeneratedBindingPropertyAttribute(string forMemberName, Type forMemberType)
        {
            this.ForMemberName = forMemberName;
            this.ForMemberType = forMemberType;
        }
    }
}
