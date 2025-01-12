using SparseInject;

public static class SparseInjectSingletonRegistrator_Depth3
{
    public static void Register(ContainerBuilder builder)
    {
        builder.Register<Dependency_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD1_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD1D1_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD1D2_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD1D3_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD1D4_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD1D5_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD2_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD2D1_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD2D2_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD2D3_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD2D4_Depth3>(Lifetime.Singleton);
        builder.Register<DependencyD2D5_Depth3>(Lifetime.Singleton);
    }
}

