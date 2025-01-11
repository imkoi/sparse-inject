using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonRegisterAndBuild_Depth5Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth5.Register(builder);

        builder.Build();
    }
}