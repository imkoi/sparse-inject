using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientFirstResolve_Depth1Scenario : Scenario
{
    public override string Name => "VContainer";
    
    private IObjectResolver _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientRegistrator_Depth1.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve(typeof(Dependency_Depth1));
    }
}