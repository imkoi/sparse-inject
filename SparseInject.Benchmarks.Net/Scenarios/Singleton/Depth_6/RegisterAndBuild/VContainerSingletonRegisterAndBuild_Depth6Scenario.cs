using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerSingletonRegisterAndBuild_Depth6Scenario : Scenario
{
    public override string Name => "VContainer";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerSingletonRegistrator_Depth6.Register(builder);

        builder.Build();
    }
}