#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientRegisterAndBuild_Depth2Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth2.Register(builder);

        builder.Build();
    }
}
#endif
