#if UNITY_2017_1_OR_NEWER
using Zenject;

public static class ZenjectTransientRegistrator_Depth1
{
    public static void Register(DiContainer builder)
    {
        builder.Bind<Dependency_Depth1>().AsTransient();
    }
}
#endif

