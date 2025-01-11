using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientBuild_Depth5Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth5.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}