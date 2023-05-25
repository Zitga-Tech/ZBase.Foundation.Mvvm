using System.Collections.Generic;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public interface IObservableCollection<T> : IReadOnlyCollection<T>
    {
        object SyncRoot { get; }

        void CollectionChanging<TInstance>(CollectionChangeEventListener<T, TInstance> listener)
            where TInstance : class;

        void CollectionChanged<TInstance>(CollectionChangeEventListener<T, TInstance> listener)
            where TInstance : class;
    }
}