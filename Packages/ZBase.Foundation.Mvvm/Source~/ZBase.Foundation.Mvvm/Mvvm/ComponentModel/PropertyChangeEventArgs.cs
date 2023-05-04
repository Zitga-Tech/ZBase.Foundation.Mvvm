using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ComponentModel
{
    public readonly struct PropertyChangeEventArgs
    {
        public readonly IObservableObject Sender;
        public readonly string PropertyName;
        public readonly Union Value;

        public PropertyChangeEventArgs(IObservableObject sender, string propertyName, in Union value)
        {
            Sender = sender;
            PropertyName = propertyName;
            Value = value;
        }
    }
}
