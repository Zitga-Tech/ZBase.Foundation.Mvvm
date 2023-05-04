namespace ZBase.Foundation.Mvvm.ComponentModel
{
    /// <summary>
    /// Notifies clients that a property value has changed.
    /// </summary>
    public interface INotifyPropertyChanged
    {
        bool PropertyChanged<TInstance>(string propertyName, PropertyChangeEventListener<TInstance> listener)
            where TInstance : class;
    }
}
