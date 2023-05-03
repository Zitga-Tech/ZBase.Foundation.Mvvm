using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm
{
    public interface IAdapter
    {
        Union Convert(in Union value);
    }
}
