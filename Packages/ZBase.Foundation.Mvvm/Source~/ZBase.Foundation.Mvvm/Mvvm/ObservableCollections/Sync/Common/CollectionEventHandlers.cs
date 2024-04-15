namespace ZBase.Foundation.Mvvm.ObservableCollections.Sync
{
    public delegate void CollectionChangingEventHandler<T>(in CollectionEventArgs<T> args);

    public delegate void CollectionChangedEventHandler<T>(in CollectionEventArgs<T> args);
}