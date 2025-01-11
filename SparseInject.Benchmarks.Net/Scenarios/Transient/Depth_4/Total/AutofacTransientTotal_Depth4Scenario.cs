using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientTotal_Depth4Scenario : Scenario
{
    public override string Name => "Autofac";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth4.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve<Dependency_Depth4>();
    }
}