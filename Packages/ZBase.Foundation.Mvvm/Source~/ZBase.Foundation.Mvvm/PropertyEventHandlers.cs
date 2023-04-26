namespace ZBase.Foundation.Mvvm
{
    public delegate void PropertyChangingEventHandler<TEventArgs>(TEventArgs args)
        where TEventArgs : IPropertyChangeEventArgs;

    public delegate void PropertyChangedEventHandler<TEventArgs>(TEventArgs args)
        where TEventArgs : IPropertyChangeEventArgs;
}
