using ZBase.Foundation.Mvvm.ComponentModel;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    public interface IBindingContext
    {
        bool IsCreated { get; }

        IObservableObject Target { get; }
    }
}
