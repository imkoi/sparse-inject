using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientRegisterAndBuild_Depth3Scenario : Scenario
{
    public override string Name => "VContainer";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientRegistrator_Depth3.Register(builder);

        builder.Build();
    }
}