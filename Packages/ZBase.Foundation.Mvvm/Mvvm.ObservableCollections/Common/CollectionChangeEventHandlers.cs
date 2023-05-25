namespace ZBase.Foundation.Mvvm.ObservableCollections
{
    public delegate void CollectionChangingEventHandler<T>(in CollectionChangeEventArgs<T> args);

    public delegate void CollectionChangedEventHandler<T>(in CollectionChangeEventArgs<T> args);
}