using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientRegisterAndBuild_Depth5Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth5.Register(builder);

        builder.Build();
    }
}