using ZBase.Foundation.Mvvm.Unions;

namespace ZBase.Foundation.Mvvm.Conversion
{
    public interface IAdapter
    {
        Union Convert(in Union value);
    }
}
