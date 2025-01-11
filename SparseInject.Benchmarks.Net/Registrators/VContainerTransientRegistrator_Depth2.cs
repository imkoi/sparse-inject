using VContainer;

public static class VContainerTransientRegistrator_Depth2
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register(typeof(Dependency_Depth2), Lifetime.Transient);
        builder.Register(typeof(DependencyD1_Depth2), Lifetime.Transient);
        builder.Register(typeof(DependencyD2_Depth2), Lifetime.Transient);
    }
}

