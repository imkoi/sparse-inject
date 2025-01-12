using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerSingletonSecondResolve_Depth5Scenario : Scenario
{
    public override string Name => "VContainer";
    
    private IObjectResolver _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        VContainerSingletonRegistrator_Depth5.Register(builder);
        
        _container = builder.Build();
        
        _container.Resolve(typeof(Dependency_Depth5));
    }

    public override void Execute()
    {
        _container.Resolve(typeof(Dependency_Depth5));
    }
}