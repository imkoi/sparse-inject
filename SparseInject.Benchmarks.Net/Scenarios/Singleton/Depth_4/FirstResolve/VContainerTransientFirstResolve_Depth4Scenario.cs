using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerSingletonFirstResolve_Depth4Scenario : Scenario
{
    public override string Name => "VContainer";
    
    private IObjectResolver _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        VContainerSingletonRegistrator_Depth4.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve(typeof(Dependency_Depth4));
    }
}