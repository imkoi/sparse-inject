using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientFirstResolve_Depth4Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private Container _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth4.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth4>();
    }
}