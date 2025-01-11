using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientRegister_Depth4Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth4.Register(builder);
    }
}
