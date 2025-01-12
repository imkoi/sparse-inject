#if NET
using LightInject;

public static class LightInjectTransientRegistrator_Depth1
{
    public static void Register(ServiceContainer builder)
    {
        builder.Register<Dependency_Depth1>();
    }
}
#endif

