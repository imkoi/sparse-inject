#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexTransientRegisterAndBuild_Depth6Scenario : Scenario
{
    public override string Name => "Reflex";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        ReflexTransientRegistrator_Depth6.Register(builder);

        builder.Build();
    }
}
#endif