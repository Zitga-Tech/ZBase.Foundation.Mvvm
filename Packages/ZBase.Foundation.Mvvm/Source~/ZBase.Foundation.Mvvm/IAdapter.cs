namespace ZBase.Foundation.Mvvm
{
    public interface IAdapter
    {
    }

    public interface IAdapter<T> : IAdapter
    {
        T Convert(object value);
    }

    public interface IBoolAdapter : IAdapter<bool> { }

    public interface IByteAdapter : IAdapter<byte> { }

    public interface ISByteAdapter : IAdapter<sbyte> { }

    public interface ICharAdapter : IAdapter<char> { }

    public interface IDecimalAdapter : IAdapter<decimal> { }

    public interface IDoubleAdapter : IAdapter<double> { }

    public interface IFloatAdapter : IAdapter<float> { }

    public interface IIntAdapter : IAdapter<int> { }

    public interface IUIntAdapter : IAdapter<uint> { }

    public interface ILongAdapter : IAdapter<long> { }

    public interface IULongAdapter : IAdapter<ulong> { }

    public interface IShortAdapter : IAdapter<short> { }

    public interface IUShortAdapter : IAdapter<ushort> { }

    public interface IStringAdapter : IAdapter<string> { }
}
