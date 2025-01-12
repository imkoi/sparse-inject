using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonSecondResolve_Depth1Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private Container _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth1.Register(builder);
        
        _container = builder.Build();
        
        _container.Resolve<Dependency_Depth1>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth1>();
    }
}