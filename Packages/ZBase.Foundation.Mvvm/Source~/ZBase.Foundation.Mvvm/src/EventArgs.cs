namespace ZBase.Foundation.Mvvm
{
    public readonly struct EventArgs
    {
        public readonly object Sender;
        public readonly ValueUnion Value;

        public EventArgs(object sender, in ValueUnion value)
        {
            Sender = sender;
            Value = value;
        }
    }

    public readonly struct PropertyChangeEventArgs
    {
        public readonly IObservableObject Sender;
        public readonly string PropertyName;
        public readonly ValueUnion Value;

        public PropertyChangeEventArgs(IObservableObject sender, string propertyName, in ValueUnion value)
        {
            Sender = sender;
            PropertyName = propertyName;
            Value = value;
        }
    }
}
