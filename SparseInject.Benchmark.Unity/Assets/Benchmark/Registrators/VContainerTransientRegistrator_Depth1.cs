#if UNITY_2017_1_OR_NEWER
using VContainer;

public static class VContainerTransientRegistrator_Depth1
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register<Dependency_Depth1>(Lifetime.Transient);
    }
}
#endif
#if NET
using VContainer;

public static class VContainerTransientRegistrator_Depth1
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register(typeof(Dependency_Depth1), Lifetime.Transient);
    }
}
#endif

