#if UNITY_2017_1_OR_NEWER
using VContainer;

public static class VContainerSingletonRegistrator_Depth2
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register<Dependency_Depth2>(Lifetime.Singleton);
        builder.Register<DependencyD1_Depth2>(Lifetime.Singleton);
        builder.Register<DependencyD2_Depth2>(Lifetime.Singleton);
    }
}
#endif
#if NET
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
#endif

