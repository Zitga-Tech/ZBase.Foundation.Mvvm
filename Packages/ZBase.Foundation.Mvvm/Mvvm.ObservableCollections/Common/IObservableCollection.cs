using System.Collections.Generic;

namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public interface IObservableCollection<T> : IReadOnlyCollection<T>
    {
        object SyncRoot { get; }

        /// <summary>
        /// Attach an instance of <see cref="CollectionEventListener{T, TInstance}"/> to the observable collection.
        /// Before the collection is changed, a notification will be sent to this <paramref name="listener"/>.
        /// </summary>
        /// <typeparam name="TInstance">The owner of the <paramref name="listener"/>.</typeparam>
        /// <param name="listener">The event listener to receive the notifications.</param>
        void AttachCollectionChangingListener<TInstance>(CollectionEventListener<T, TInstance> listener)
            where TInstance : class;

        /// <summary>
        /// Attach an instance of <see cref="CollectionEventListener{T, TInstance}"/> to the observable collection.
        /// After the collection is changed, a notification will be sent to this <paramref name="listener"/>.
        /// </summary>
        /// <typeparam name="TInstance">The owner of the <paramref name="listener"/>.</typeparam>
        /// <param name="listener">The event listener to receive the notifications.</param>
        void AttachCollectionChangedListener<TInstance>(CollectionEventListener<T, TInstance> listener)
            where TInstance : class;

        /// <summary>
        /// Force the observable collection to send its current state to <paramref name="listener"/>.
        /// </summary>
        /// <typeparam name="TInstance">The owner of the <paramref name="listener"/>.</typeparam>
        /// <param name="listener">The event listener to receive the notifications.</param>
        void NotifyCollectionChanged<TInstance>(CollectionEventListener<T, TInstance> listener)
            where TInstance : class;

        /// <summary>
        /// Force the observable collection to send its current state to all listeners.
        /// </summary>
        void NotifyCollectionChanged();
    }
}