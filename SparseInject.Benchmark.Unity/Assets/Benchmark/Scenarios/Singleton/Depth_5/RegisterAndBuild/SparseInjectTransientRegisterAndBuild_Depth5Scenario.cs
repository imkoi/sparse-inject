using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonRegisterAndBuild_Depth5Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth5.Register(builder);

        builder.Build();
    }
}