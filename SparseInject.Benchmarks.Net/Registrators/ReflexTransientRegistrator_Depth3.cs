#if UNITY_2017_1_OR_NEWER
using Reflex.Core;

public static class ReflexTransientRegistrator_Depth3
{
    public static void Register(ContainerBuilder builder)
    {
        builder.AddTransient(typeof(Dependency_Depth3));
        builder.AddTransient(typeof(DependencyD1_Depth3));
        builder.AddTransient(typeof(DependencyD1D1_Depth3));
        builder.AddTransient(typeof(DependencyD1D2_Depth3));
        builder.AddTransient(typeof(DependencyD1D3_Depth3));
        builder.AddTransient(typeof(DependencyD1D4_Depth3));
        builder.AddTransient(typeof(DependencyD1D5_Depth3));
        builder.AddTransient(typeof(DependencyD2_Depth3));
        builder.AddTransient(typeof(DependencyD2D1_Depth3));
        builder.AddTransient(typeof(DependencyD2D2_Depth3));
        builder.AddTransient(typeof(DependencyD2D3_Depth3));
        builder.AddTransient(typeof(DependencyD2D4_Depth3));
        builder.AddTransient(typeof(DependencyD2D5_Depth3));
    }
}
#endif

