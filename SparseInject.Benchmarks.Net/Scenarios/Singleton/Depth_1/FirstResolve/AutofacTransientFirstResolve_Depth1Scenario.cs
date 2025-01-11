using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonFirstResolve_Depth1Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private IContainer _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth1.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth1>();
    }
}