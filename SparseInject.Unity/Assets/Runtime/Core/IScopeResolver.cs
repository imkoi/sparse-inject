namespace SparseInject
{
    public interface IScopeResolver
    {
        T Resolve<T>() where T : class;
    }
}