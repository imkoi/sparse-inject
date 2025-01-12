#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonRegisterAndBuild_Depth4Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth4.Register(builder);

        builder.Build();
    }
}
#endif
