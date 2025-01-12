using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerSingletonSecondResolve_Depth6Scenario : Scenario
{
    public override string Name => "VContainer";
    
    private IObjectResolver _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        VContainerSingletonRegistrator_Depth6.Register(builder);
        
        _container = builder.Build();
        
        _container.Resolve(typeof(Dependency_Depth6));
    }

    public override void Execute()
    {
        _container.Resolve(typeof(Dependency_Depth6));
    }
}