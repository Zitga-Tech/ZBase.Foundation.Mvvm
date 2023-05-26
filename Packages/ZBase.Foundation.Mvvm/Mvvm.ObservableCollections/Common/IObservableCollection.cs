using System.Collections.Generic;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public interface IObservableCollection<T> : IReadOnlyCollection<T>
    {
        object SyncRoot { get; }

        void CollectionChanging<TInstance>(CollectionEventListener<T, TInstance> listener)
            where TInstance : class;

        void CollectionChanged<TInstance>(CollectionEventListener<T, TInstance> listener)
            where TInstance : class;
    }
}