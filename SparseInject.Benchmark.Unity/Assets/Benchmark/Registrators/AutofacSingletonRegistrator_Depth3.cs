#if NET
using Autofac;

public static class AutofacSingletonRegistrator_Depth3
{
    public static void Register(ContainerBuilder builder)
    {
        builder.RegisterType<Dependency_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD1_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD1D1_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD1D2_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD1D3_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD1D4_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD1D5_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD2_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD2D1_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD2D2_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD2D3_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD2D4_Depth3>().SingleInstance();
        builder.RegisterType<DependencyD2D5_Depth3>().SingleInstance();
    }
}
#endif

