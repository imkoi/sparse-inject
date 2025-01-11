using VContainer;

public static class VContainerSingletonRegistrator_Depth2
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register(typeof(Dependency_Depth2), Lifetime.Singleton);
        builder.Register(typeof(DependencyD1_Depth2), Lifetime.Singleton);
        builder.Register(typeof(DependencyD2_Depth2), Lifetime.Singleton);
    }
}

