namespace ZBase.Foundation.Mvvm
{
    public readonly struct PropertyChangeEventArgs
    {
        public readonly IObservableObject Sender;
        public readonly string PropertyName;
        public readonly ValueUnion Value;

        public PropertyChangeEventArgs(IObservableObject sender, string propertyName, ValueUnion value)
        {
            Sender = sender;
            PropertyName = propertyName;
            Value = value;
        }
    }
}
