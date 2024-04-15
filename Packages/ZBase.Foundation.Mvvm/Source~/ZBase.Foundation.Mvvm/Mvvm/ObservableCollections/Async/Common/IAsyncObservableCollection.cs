using System.Collections.Generic;

#if ENABLE_UNITASK
using Task = Cysharp.Threading.Tasks.UniTask;
#else
using Task = System.Threading.Tasks.Task;
#endif

namespace ZBase.Foundation.Mvvm.ObservableCollections.Async
{
    public interface IAsyncObservableCollection<T> : IReadOnlyCollection<T>
    {
        object SyncRoot { get; }

        /// <summary>
        /// Attach an instance of <see cref="AsyncCollectionEventListener{T, TInstance}"/> to the observable collection.
        /// Before the collection is changed, a notification will be sent to this <paramref name="listener"/>.
        /// </summary>
        /// <typeparam name="TInstance">The owner of the <paramref name="listener"/>.</typeparam>
        /// <param name="listener">The event listener to receive the notifications.</param>
        void AttachCollectionChangingListener<TInstance>(AsyncCollectionEventListener<T, TInstance> listener)
            where TInstance : class;

        /// <summary>
        /// Attach an instance of <see cref="AsyncCollectionEventListener{T, TInstance}"/> to the observable collection.
        /// After the collection is changed, a notification will be sent to this <paramref name="listener"/>.
        /// </summary>
        /// <typeparam name="TInstance">The owner of the <paramref name="listener"/>.</typeparam>
        /// <param name="listener">The event listener to receive the notifications.</param>
        void AttachCollectionChangedListener<TInstance>(AsyncCollectionEventListener<T, TInstance> listener)
            where TInstance : class;

        /// <summary>
        /// Force the observable collection to send its current state to <paramref name="listener"/>.
        /// </summary>
        /// <typeparam name="TInstance">The owner of the <paramref name="listener"/>.</typeparam>
        /// <param name="listener">The event listener to receive the notifications.</param>
        Task NotifyCollectionChanged<TInstance>(AsyncCollectionEventListener<T, TInstance> listener)
            where TInstance : class;

        /// <summary>
        /// Force the observable collection to send its current state to all listeners.
        /// </summary>
        Task NotifyCollectionChanged();
    }
}