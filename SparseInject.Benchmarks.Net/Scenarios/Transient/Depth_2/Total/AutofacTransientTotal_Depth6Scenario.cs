#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientTotal_Depth2Scenario : Scenario
{
    public override string Name => "Autofac";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth2.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve<Dependency_Depth2>();
    }
}
#endif
