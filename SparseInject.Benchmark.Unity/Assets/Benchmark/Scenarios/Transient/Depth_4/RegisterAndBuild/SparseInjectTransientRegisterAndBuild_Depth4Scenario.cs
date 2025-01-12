using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientRegisterAndBuild_Depth4Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth4.Register(builder);

        builder.Build();
    }
}