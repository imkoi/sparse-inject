using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientBuild_Depth3Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth3.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}