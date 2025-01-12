#if NET
using LightInject;

public static class LightInjectSingletonRegistrator_Depth1
{
    public static void Register(ServiceContainer builder)
    {
        builder.RegisterSingleton<Dependency_Depth1>();
    }
}
#endif

