using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.ViewBinding
{
    public interface IAdapter
    {
        Union Convert(in Union value);
    }
}
