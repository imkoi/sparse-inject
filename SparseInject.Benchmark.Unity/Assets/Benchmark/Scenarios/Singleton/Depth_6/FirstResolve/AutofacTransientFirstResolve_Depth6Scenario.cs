#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonFirstResolve_Depth6Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private IContainer _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth6.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth6>();
    }
}
#endif
