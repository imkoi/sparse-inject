using SparseInject;

public static class SparseInjectTransientRegistrator_Depth2
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register<Dependency_Depth2>();
        builder.Register<DependencyD1_Depth2>();
        builder.Register<DependencyD2_Depth2>();
    }
}

