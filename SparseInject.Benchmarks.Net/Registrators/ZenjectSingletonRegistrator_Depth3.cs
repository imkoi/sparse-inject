#if UNITY_2017_1_OR_NEWER
using Zenject;

public static class ZenjectSingletonRegistrator_Depth3
{
    public static void Register(DiContainer builder)
    {
        builder.Bind<Dependency_Depth3>().AsSingle();
        builder.Bind<DependencyD1_Depth3>().AsSingle();
        builder.Bind<DependencyD1D1_Depth3>().AsSingle();
        builder.Bind<DependencyD1D2_Depth3>().AsSingle();
        builder.Bind<DependencyD1D3_Depth3>().AsSingle();
        builder.Bind<DependencyD1D4_Depth3>().AsSingle();
        builder.Bind<DependencyD1D5_Depth3>().AsSingle();
        builder.Bind<DependencyD2_Depth3>().AsSingle();
        builder.Bind<DependencyD2D1_Depth3>().AsSingle();
        builder.Bind<DependencyD2D2_Depth3>().AsSingle();
        builder.Bind<DependencyD2D3_Depth3>().AsSingle();
        builder.Bind<DependencyD2D4_Depth3>().AsSingle();
        builder.Bind<DependencyD2D5_Depth3>().AsSingle();
    }
}
#endif

