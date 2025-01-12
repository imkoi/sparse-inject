using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerSingletonTotal_Depth5Scenario : Scenario
{
    public override string Name => "VContainer";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerSingletonRegistrator_Depth5.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve(typeof(Dependency_Depth5));
    }
}