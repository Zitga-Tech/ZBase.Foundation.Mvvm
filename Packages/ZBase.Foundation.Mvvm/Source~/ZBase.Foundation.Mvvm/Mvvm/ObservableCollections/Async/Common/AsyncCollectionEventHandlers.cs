#if ENABLE_UNITASK
using Task = Cysharp.Threading.Tasks.UniTask;
#else
using Task = System.Threading.Tasks.Task;
#endif

namespace ZBase.Foundation.Mvvm.ObservableCollections.Async
{
    public delegate Task AsyncCollectionChangingEventHandler<T>(AsyncCollectionEventArgs<T> args);

    public delegate Task AsyncCollectionChangedEventHandler<T>(AsyncCollectionEventArgs<T> args);
}