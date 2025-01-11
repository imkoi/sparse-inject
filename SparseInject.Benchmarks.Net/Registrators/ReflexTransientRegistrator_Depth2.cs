#if UNITY_2017_1_OR_NEWER
using Reflex.Core;

public static class ReflexTransientRegistrator_Depth2
{
    public static void Register(ContainerBuilder builder)
    {
        builder.AddTransient(typeof(Dependency_Depth2));
        builder.AddTransient(typeof(DependencyD1_Depth2));
        builder.AddTransient(typeof(DependencyD2_Depth2));
    }
}
#endif

