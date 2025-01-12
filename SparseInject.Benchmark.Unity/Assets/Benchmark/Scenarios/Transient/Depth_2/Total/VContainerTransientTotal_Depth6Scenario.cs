using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientTotal_Depth2Scenario : Scenario
{
    public override string Name => "VContainer";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientRegistrator_Depth2.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve(typeof(Dependency_Depth2));
    }
}