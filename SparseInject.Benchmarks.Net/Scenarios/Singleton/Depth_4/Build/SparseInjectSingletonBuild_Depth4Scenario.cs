using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonBuild_Depth4Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth4.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}