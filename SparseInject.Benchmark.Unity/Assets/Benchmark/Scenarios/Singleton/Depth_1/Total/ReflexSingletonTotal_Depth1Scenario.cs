#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexSingletonTotal_Depth1Scenario : Scenario
{
    public override string Name => "Reflex";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        ReflexSingletonRegistrator_Depth1.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve<Dependency_Depth1>();
    }
}
#endif