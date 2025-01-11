#if NET
using LightInject;

public static class LightInjectSingletonRegistrator_Depth2
{
    public static void Register(ServiceContainer builder)
    {
        builder.RegisterSingleton<Dependency_Depth2>();
        builder.RegisterSingleton<DependencyD1_Depth2>();
        builder.RegisterSingleton<DependencyD2_Depth2>();
    }
}
#endif

