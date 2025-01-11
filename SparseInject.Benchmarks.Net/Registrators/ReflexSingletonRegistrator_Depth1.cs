#if UNITY_2017_1_OR_NEWER
using Reflex.Core;

public static class ReflexSingletonRegistrator_Depth1
{
    public static void Register(ContainerBuilder builder)
    {
        builder.AddSingleton(typeof(Dependency_Depth1));
    }
}
#endif

