using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientRegisterAndBuild_Depth4Scenario : Scenario
{
    public override string Name => "VContainer";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientRegistrator_Depth4.Register(builder);

        builder.Build();
    }
}