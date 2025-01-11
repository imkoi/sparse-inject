using SparseInject;

public static class SparseInjectSingletonRegistrator_Depth2
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register<Dependency_Depth2>(Lifetime.Singleton);
        builder.Register<DependencyD1_Depth2>(Lifetime.Singleton);
        builder.Register<DependencyD2_Depth2>(Lifetime.Singleton);
    }
}

