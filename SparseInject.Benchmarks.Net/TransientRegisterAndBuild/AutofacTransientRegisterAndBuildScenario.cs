using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientRegisterAndBuildScenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientContainerRegistrator.Register(builder);

        builder.Build();
    }
}