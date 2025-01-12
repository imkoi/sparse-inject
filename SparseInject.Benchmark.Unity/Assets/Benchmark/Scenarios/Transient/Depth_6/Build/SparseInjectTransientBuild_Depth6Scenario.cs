using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientBuild_Depth6Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth6.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}