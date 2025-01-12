using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerSingletonRegisterAndBuild_Depth4Scenario : Scenario
{
    public override string Name => "VContainer";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerSingletonRegistrator_Depth4.Register(builder);

        builder.Build();
    }
}