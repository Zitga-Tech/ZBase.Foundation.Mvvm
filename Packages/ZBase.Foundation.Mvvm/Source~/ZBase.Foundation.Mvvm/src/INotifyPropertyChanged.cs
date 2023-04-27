namespace ZBase.Foundation.Mvvm
{
    /// <summary>
    /// Notifies clients that a property value has changed.
    /// </summary>
    public interface INotifyPropertyChanged
    {
        public void PropertyChanged<TInstance>(string propertyName, PropertyChangeEventListener<TInstance> listener)
           where TInstance : class
        { }
    }
}
