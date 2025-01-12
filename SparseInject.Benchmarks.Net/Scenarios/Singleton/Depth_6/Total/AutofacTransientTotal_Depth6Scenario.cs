#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonTotal_Depth6Scenario : Scenario
{
    public override string Name => "Autofac";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth6.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve<Dependency_Depth6>();
    }
}
#endif
