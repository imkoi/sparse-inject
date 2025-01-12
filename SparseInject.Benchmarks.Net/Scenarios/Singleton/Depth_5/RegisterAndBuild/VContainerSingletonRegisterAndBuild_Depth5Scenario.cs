using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerSingletonRegisterAndBuild_Depth5Scenario : Scenario
{
    public override string Name => "VContainer";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerSingletonRegistrator_Depth5.Register(builder);

        builder.Build();
    }
}