using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientBuild_Depth1Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth1.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}