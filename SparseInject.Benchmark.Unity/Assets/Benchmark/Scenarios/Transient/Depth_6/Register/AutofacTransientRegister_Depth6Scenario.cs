#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientRegister_Depth6Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth6.Register(builder);
    }
}
#endif
