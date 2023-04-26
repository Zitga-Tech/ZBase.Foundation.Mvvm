namespace ZBase.Foundation.Mvvm
{
    /// <summary>
    /// Notifies clients that a property value has changed.
    /// </summary>
    public interface INotifyPropertyChanged
    {
        public void PropertyChanged<TInstance, TEventArgs>(string propertyName, PropertyChangeEventListener<TInstance, TEventArgs> listener)
           where TInstance : class
           where TEventArgs : IPropertyChangeEventArgs
        { }
    }
}
