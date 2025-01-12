#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexTransientTotal_Depth4Scenario : Scenario
{
    public override string Name => "Reflex";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        ReflexTransientRegistrator_Depth4.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve<Dependency_Depth4>();
    }
}
#endif