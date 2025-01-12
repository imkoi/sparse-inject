using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonTotal_Depth6Scenario : Scenario
{
    public override string Name => "SparseInject";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth6.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve<Dependency_Depth6>();
    }
}