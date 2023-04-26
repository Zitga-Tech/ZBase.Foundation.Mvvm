namespace ZBase.Foundation.Mvvm
{
    public readonly struct PropertyChangeEventArgs<T> : IPropertyChangeEventArgs
    {
        public readonly IObservableObject Sender;
        public readonly string PropertyName;
        public readonly T Value;

        public PropertyChangeEventArgs(IObservableObject sender, string propertyName, T value)
        {
            Sender = sender;
            PropertyName = propertyName;
            Value = value;
        }
    }
}
