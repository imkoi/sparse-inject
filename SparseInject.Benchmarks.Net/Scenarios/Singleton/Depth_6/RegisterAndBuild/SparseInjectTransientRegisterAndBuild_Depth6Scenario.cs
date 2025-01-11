using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonRegisterAndBuild_Depth6Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth6.Register(builder);

        builder.Build();
    }
}