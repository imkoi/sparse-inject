#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonRegister_Depth1Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth1.Register(builder);
    }
}
#endif
