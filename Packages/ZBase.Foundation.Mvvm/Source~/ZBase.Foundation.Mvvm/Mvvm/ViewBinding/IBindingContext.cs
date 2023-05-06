using ZBase.Foundation.Mvvm.ComponentModel;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    public interface IBindingContext
    {
        IObservableObject Target { get; }
    }
}
