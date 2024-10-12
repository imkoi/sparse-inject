namespace CleanResolver
{
    public interface IScopeResolver
    {
        public T Resolve<T>() where T : class;
    }
}