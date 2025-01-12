#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexSingletonRegisterAndBuild_Depth1Scenario : Scenario
{
    public override string Name => "Reflex";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        ReflexSingletonRegistrator_Depth1.Register(builder);

        builder.Build();
    }
}
#endif