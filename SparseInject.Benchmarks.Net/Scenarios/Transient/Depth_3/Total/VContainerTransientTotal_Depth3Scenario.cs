using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientTotal_Depth3Scenario : Scenario
{
    public override string Name => "VContainer";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientRegistrator_Depth3.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve(typeof(Dependency_Depth3));
    }
}