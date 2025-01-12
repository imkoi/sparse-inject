#if NET
using Autofac;

public static class AutofacSingletonRegistrator_Depth2
{
    public static void Register(ContainerBuilder builder)
    {
        builder.RegisterType<Dependency_Depth2>().SingleInstance();
        builder.RegisterType<DependencyD1_Depth2>().SingleInstance();
        builder.RegisterType<DependencyD2_Depth2>().SingleInstance();
    }
}
#endif

