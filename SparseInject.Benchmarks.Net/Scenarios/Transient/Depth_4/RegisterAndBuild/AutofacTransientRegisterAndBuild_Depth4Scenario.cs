using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientRegisterAndBuild_Depth4Scenario : Scenario
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth4.Register(builder);

        builder.Build();
    }
}