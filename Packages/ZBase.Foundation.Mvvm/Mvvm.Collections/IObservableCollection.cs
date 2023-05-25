using ZBase.Foundation.Mvvm.ComponentModel;

namespace ZBase.Foundation.Mvvm.Collections
{
    public interface IObservableCollection<T> : IObservableObject
    {
        object SyncRoot { get; }

        bool CollectionChanged<TInstance>(CollectionAction action, CollectionChangedEventListener<T, TInstance> listener)
            where TInstance : class;
    }
}