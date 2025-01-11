#if UNITY_2017_1_OR_NEWER
using Reflex.Core;

public static class ReflexTransientRegistrator_Depth1
{
    public static void Register(ContainerBuilder builder)
    {
        builder.AddTransient(typeof(Dependency_Depth1));
    }
}
#endif

