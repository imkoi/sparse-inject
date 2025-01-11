using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientSecondResolve_Depth6Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private IContainer _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth6.Register(builder);
        
        _container = builder.Build();
        
        _container.Resolve<Dependency_Depth6>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth6>();
    }
}