namespace ZBase.Foundation.Mvvm
{
    public interface IDataContext { }

    public interface IDataContext<T> : IDataContext
        where T : class, IObservableObject
    {
        public T ViewModel { get => default; }
    }
}
