#if UNITY_2017_1_OR_NEWER
using VContainer;

public static class VContainerTransientRegistrator_Depth2
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register<Dependency_Depth2>(Lifetime.Transient);
        builder.Register<DependencyD1_Depth2>(Lifetime.Transient);
        builder.Register<DependencyD2_Depth2>(Lifetime.Transient);
    }
}
#endif
#if NET
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
#endif

