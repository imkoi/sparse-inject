using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientBuild_Depth2Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth2.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}