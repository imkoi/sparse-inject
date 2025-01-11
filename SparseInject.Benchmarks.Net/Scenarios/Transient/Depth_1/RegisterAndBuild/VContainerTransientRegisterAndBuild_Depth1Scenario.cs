using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientRegisterAndBuild_Depth1Scenario : Scenario
{
    public override string Name => "VContainer";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientRegistrator_Depth1.Register(builder);

        builder.Build();
    }
}