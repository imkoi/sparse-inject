#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexSingletonBuild_Depth6Scenario : Scenario
{
    public override string Name => "Reflex";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        ReflexSingletonRegistrator_Depth6.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}
#endif