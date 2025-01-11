using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientRegister_Depth2Scenario : Scenario
{
    public override string Name => "VContainer";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientRegistrator_Depth2.Register(builder);
    }
}