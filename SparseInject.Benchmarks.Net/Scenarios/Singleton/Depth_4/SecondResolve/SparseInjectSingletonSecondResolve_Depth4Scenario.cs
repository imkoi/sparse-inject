using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonSecondResolve_Depth4Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private Container _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth4.Register(builder);
        
        _container = builder.Build();
        
        _container.Resolve<Dependency_Depth4>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth4>();
    }
}