namespace ZBase.Foundation.Mvvm.Unions
{
    public interface IUnionConverter<T>
    {
        public Union ToUnion(T value)
        {
            return default;
        }

        public Union<T> ToUnionT(T value)
        {
            return default;
        }

        public bool TryGetValue(in Union union, out T result)
        {
            result = default;
            return false;
        }

        public bool TrySetValue(in Union union, ref T dest)
        {
            return false;
        }
    }
}
