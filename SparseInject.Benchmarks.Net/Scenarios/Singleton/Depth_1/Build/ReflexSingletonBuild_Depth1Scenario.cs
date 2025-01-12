#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexSingletonBuild_Depth1Scenario : Scenario
{
    public override string Name => "Reflex";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        ReflexSingletonRegistrator_Depth1.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}
#endif