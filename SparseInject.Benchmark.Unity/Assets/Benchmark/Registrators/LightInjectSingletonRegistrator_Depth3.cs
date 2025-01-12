#if NET
using LightInject;

public static class LightInjectSingletonRegistrator_Depth3
{
    public static void Register(ServiceContainer builder)
    {
        builder.RegisterSingleton<Dependency_Depth3>();
        builder.RegisterSingleton<DependencyD1_Depth3>();
        builder.RegisterSingleton<DependencyD1D1_Depth3>();
        builder.RegisterSingleton<DependencyD1D2_Depth3>();
        builder.RegisterSingleton<DependencyD1D3_Depth3>();
        builder.RegisterSingleton<DependencyD1D4_Depth3>();
        builder.RegisterSingleton<DependencyD1D5_Depth3>();
        builder.RegisterSingleton<DependencyD2_Depth3>();
        builder.RegisterSingleton<DependencyD2D1_Depth3>();
        builder.RegisterSingleton<DependencyD2D2_Depth3>();
        builder.RegisterSingleton<DependencyD2D3_Depth3>();
        builder.RegisterSingleton<DependencyD2D4_Depth3>();
        builder.RegisterSingleton<DependencyD2D5_Depth3>();
    }
}
#endif

