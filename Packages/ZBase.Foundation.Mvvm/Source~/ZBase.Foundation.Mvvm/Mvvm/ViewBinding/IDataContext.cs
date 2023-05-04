using ZBase.Foundation.Mvvm.ComponentModel;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    public interface IDataContext
    {
        public IObservableObject ViewModel { get => default; }
    }
}
