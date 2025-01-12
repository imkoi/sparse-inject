#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientSecondResolve_Depth2Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private IContainer _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth2.Register(builder);
        
        _container = builder.Build();
        
        _container.Resolve<Dependency_Depth2>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth2>();
    }
}
#endif
