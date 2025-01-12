#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientRegister_Depth1Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth1.Register(builder);
    }
}
#endif
