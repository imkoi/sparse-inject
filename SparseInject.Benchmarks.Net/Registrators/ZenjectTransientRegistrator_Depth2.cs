#if UNITY_2017_1_OR_NEWER
using Zenject;

public static class ZenjectTransientRegistrator_Depth2
{
    public static void Register(DiContainer builder)
    {
        builder.Bind<Dependency_Depth2>().AsTransient();
        builder.Bind<DependencyD1_Depth2>().AsTransient();
        builder.Bind<DependencyD2_Depth2>().AsTransient();
    }
}
#endif

