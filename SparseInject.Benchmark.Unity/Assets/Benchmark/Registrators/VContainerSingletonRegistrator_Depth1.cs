using VContainer;

public static class VContainerSingletonRegistrator_Depth1
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register(typeof(Dependency_Depth1), Lifetime.Singleton);
    }
}

