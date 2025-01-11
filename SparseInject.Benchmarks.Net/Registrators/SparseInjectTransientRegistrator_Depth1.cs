using SparseInject;

public static class SparseInjectTransientRegistrator_Depth1
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register<Dependency_Depth1>();
    }
}

