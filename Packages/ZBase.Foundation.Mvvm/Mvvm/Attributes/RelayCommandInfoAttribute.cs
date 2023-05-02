using System;

namespace ZBase.Foundation.Mvvm
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public sealed class RelayCommandInfoAttribute : Attribute
    {
        public string CommandName { get; }

        public Type ParameterType { get; }

        public RelayCommandInfoAttribute(string commandName) : this(commandName, null)
        { }

        public RelayCommandInfoAttribute(string commandName, Type parameterType)
        {
            this.CommandName = commandName;
            this.ParameterType = parameterType;
        }
    }
}
