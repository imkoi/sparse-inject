using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientRegisterAndBuild_Depth3Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth3.Register(builder);

        builder.Build();
    }
}