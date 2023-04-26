namespace ZBase.Foundation.Mvvm
{
    /// <summary>
    /// Notifies clients that a property value is changing.
    /// </summary>
    public interface INotifyPropertyChanging
    {
        public void PropertyChanging<TInstance, TEventArgs>(string propertyName, PropertyChangeEventListener<TInstance, TEventArgs> listener)
            where TInstance : class
            where TEventArgs : IPropertyChangeEventArgs
        { }
    }
}
