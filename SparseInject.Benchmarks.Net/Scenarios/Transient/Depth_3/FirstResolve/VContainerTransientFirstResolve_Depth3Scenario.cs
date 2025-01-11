using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientFirstResolve_Depth3Scenario : Scenario
{
    public override string Name => "VContainer";
    
    private IObjectResolver _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientRegistrator_Depth3.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve(typeof(Dependency_Depth3));
    }
}