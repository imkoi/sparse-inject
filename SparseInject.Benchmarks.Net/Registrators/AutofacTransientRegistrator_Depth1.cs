#if NET
using Autofac;

public static class AutofacTransientRegistrator_Depth1
{
    public static void Register(ContainerBuilder builder)
    {
        builder.RegisterType<Dependency_Depth1>();
    }
}
#endif

