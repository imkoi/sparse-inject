using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientTotalScenario : Scenario
{
    public override string Name => "VContainer";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientContainerRegistrator.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve(typeof(Class0));
    }
}