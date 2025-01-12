#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonRegister_Depth2Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth2.Register(builder);
    }
}
#endif
