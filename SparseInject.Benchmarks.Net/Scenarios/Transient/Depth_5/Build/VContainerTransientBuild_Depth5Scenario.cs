using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientBuild_Depth5Scenario : Scenario
{
    public override string Name => "VContainer";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        VContainerTransientRegistrator_Depth5.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}