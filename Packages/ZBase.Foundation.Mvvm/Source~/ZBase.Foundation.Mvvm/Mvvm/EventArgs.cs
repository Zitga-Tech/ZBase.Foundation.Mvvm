using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm
{
    public readonly struct EventArgs
    {
        public readonly object Sender;
        public readonly Union Value;

        public EventArgs(object sender, in Union value)
        {
            Sender = sender;
            Value = value;
        }
    }

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
