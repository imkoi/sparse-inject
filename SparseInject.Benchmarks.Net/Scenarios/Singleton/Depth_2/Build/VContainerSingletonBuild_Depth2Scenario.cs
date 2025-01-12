using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerSingletonBuild_Depth2Scenario : Scenario
{
    public override string Name => "VContainer";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        VContainerSingletonRegistrator_Depth2.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}