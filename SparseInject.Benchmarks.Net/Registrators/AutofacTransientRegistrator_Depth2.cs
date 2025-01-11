#if NET
using Autofac;

public static class AutofacTransientRegistrator_Depth2
{
    public static void Register(ContainerBuilder builder)
    {
        builder.RegisterType<Dependency_Depth2>();
        builder.RegisterType<DependencyD1_Depth2>();
        builder.RegisterType<DependencyD2_Depth2>();
    }
}
#endif

