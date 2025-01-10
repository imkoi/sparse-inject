using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientRegisterScenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientContainerRegistrator.Register(builder);
    }
}
