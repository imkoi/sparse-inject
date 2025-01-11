using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientRegisterAndBuild_Depth6Scenario : Scenario
{
    public override string Name => "VContainer";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientRegistrator_Depth6.Register(builder);

        builder.Build();
    }
}