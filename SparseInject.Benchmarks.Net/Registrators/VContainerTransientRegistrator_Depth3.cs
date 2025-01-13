#if UNITY_2017_1_OR_NEWER
using VContainer;

public static class VContainerTransientRegistrator_Depth3
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register<Dependency_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD1_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD1D1_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD1D2_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD1D3_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD1D4_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD1D5_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD2_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD2D1_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD2D2_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD2D3_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD2D4_Depth3>(Lifetime.Transient);
        builder.Register<DependencyD2D5_Depth3>(Lifetime.Transient);
    }
}
#endif
#if NET
using VContainer;

public static class VContainerTransientRegistrator_Depth3
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register(typeof(Dependency_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD1_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD1D1_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD1D2_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD1D3_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD1D4_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD1D5_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD2_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD2D1_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD2D2_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD2D3_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD2D4_Depth3), Lifetime.Transient);
        builder.Register(typeof(DependencyD2D5_Depth3), Lifetime.Transient);
    }
}
#endif

