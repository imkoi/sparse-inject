#if UNITY_2017_1_OR_NEWER
using Reflex.Core;

public static class ReflexSingletonRegistrator_Depth2
{
    public static void Register(ContainerBuilder builder)
    {
        builder.AddSingleton(typeof(Dependency_Depth2));
        builder.AddSingleton(typeof(DependencyD1_Depth2));
        builder.AddSingleton(typeof(DependencyD2_Depth2));
    }
}
#endif

