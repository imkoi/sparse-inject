using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientRegisterAndBuild_Depth1Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth1.Register(builder);

        builder.Build();
    }
}