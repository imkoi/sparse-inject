#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientSecondResolve_Depth1Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private IContainer _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth1.Register(builder);
        
        _container = builder.Build();
        
        _container.Resolve<Dependency_Depth1>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth1>();
    }
}
#endif
