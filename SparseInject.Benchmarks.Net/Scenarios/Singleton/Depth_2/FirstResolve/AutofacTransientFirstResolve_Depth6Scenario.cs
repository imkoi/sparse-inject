#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonFirstResolve_Depth2Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private IContainer _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth2.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth2>();
    }
}
#endif
