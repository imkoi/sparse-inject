#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonRegisterAndBuild_Depth6Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth6.Register(builder);

        builder.Build();
    }
}
#endif
