#if NET
using LightInject;

public static class LightInjectTransientRegistrator_Depth2
{
    public static void Register(ServiceContainer builder)
    {
        builder.Register<Dependency_Depth2>();
        builder.Register<DependencyD1_Depth2>();
        builder.Register<DependencyD2_Depth2>();
    }
}
#endif

