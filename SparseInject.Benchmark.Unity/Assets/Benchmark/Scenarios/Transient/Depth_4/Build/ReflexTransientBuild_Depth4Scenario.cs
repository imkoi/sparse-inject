#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexTransientBuild_Depth4Scenario : Scenario
{
    public override string Name => "Reflex";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        ReflexTransientRegistrator_Depth4.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}
#endif