public interface IPooled<T>
{
    void SetPool(IPool<T> pool);
    void OnReturnToPool();
}