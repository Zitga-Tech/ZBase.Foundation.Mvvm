namespace ZBase.Foundation.Mvvm.Unions
{
    public interface IUnionConverter<T>
    {
        Union ToUnion(T value);

        Union<T> ToUnionT(T value);

        bool TryGetValue(in Union union, out T result);

        bool TrySetValueTo(in Union union, ref T dest);

        string ToString(in Union union);
    }
}
