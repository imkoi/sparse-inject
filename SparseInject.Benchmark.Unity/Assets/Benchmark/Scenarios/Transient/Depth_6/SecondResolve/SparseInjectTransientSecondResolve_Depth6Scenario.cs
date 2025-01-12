using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientSecondResolve_Depth6Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    private Container _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth6.Register(builder);
        
        _container = builder.Build();
        
        _container.Resolve<Dependency_Depth6>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth6>();
    }
}