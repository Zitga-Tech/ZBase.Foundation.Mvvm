using ZBase.Foundation.Mvvm.ComponentModel;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    public interface IDataContext
    {
        IObservableObject ViewModel { get; }
    }
}
