using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonTotal_Depth3Scenario : Scenario
{
    public override string Name => "SparseInject";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth3.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve<Dependency_Depth3>();
    }
}