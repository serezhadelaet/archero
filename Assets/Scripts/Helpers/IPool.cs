public interface IPool<T>
{
    void Return(T item);
}