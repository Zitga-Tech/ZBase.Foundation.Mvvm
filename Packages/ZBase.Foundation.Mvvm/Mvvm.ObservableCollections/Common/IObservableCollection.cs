using System.Collections.Generic;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public interface IObservableCollection<T> : IReadOnlyCollection<T>
    {
        object SyncRoot { get; }

        bool CollectionChanging<TInstance>(CollectionAction action, CollectionChangeEventListener<T, TInstance> listener)
            where TInstance : class;

        bool CollectionChanged<TInstance>(CollectionAction action, CollectionChangeEventListener<T, TInstance> listener)
            where TInstance : class;
    }
}