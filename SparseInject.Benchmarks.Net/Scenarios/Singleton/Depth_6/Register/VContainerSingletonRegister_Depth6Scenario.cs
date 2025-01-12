using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerSingletonRegister_Depth6Scenario : Scenario
{
    public override string Name => "VContainer";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerSingletonRegistrator_Depth6.Register(builder);
    }
}