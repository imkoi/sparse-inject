using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonSecondResolve_Depth3Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private Container _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth3.Register(builder);
        
        _container = builder.Build();
        
        _container.Resolve<Dependency_Depth3>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth3>();
    }
}