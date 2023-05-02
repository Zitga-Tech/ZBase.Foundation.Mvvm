using System;

namespace ZBase.Foundation.Mvvm
{
    public readonly struct ObservableProperty
    {
        public readonly string Name;
        public readonly Type ReturnType;

        public ObservableProperty(string name, Type returnType)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ReturnType = returnType ?? throw new ArgumentNullException(nameof(returnType));
        }
    }
}
