#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientFirstResolve_Depth4Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private IContainer _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth4.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth4>();
    }
}
#endif
