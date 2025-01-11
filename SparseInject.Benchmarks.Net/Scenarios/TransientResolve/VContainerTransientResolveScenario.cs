using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientResolveScenario : Scenario
{
    public override string Name => "VContainer";
    
    private IObjectResolver _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientContainerRegistrator.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve(typeof(Class0));
    }
}