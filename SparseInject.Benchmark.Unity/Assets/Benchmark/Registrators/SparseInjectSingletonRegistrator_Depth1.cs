using SparseInject;

public static class SparseInjectSingletonRegistrator_Depth1
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register<Dependency_Depth1>(Lifetime.Singleton);
    }
}

