#if NET
using Autofac;

public static class AutofacSingletonRegistrator_Depth1
{
    public static void Register(ContainerBuilder builder)
    {
        builder.RegisterType<Dependency_Depth1>().SingleInstance();
    }
}
#endif

