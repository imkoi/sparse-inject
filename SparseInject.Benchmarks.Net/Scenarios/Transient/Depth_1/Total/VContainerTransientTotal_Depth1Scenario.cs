using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientTotal_Depth1Scenario : Scenario
{
    public override string Name => "VContainer";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientRegistrator_Depth1.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve(typeof(Dependency_Depth1));
    }
}