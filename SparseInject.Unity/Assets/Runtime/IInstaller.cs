namespace SparseInject
{
    public interface IInstaller
    {
        void InstallBindings(IScopeBuilder containerBuilder);
    }
}