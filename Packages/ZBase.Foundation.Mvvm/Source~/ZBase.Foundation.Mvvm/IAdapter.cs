namespace ZBase.Foundation.Mvvm
{
    public interface IAdapter
    {
        ValueUnion Convert(in ValueUnion value);
    }
}
