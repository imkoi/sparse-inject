using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonFirstResolve_Depth2Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private Container _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth2.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth2>();
    }
}