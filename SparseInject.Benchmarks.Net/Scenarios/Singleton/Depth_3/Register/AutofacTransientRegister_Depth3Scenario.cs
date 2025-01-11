using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonRegister_Depth3Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth3.Register(builder);
    }
}
