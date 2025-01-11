using SparseInject.BenchmarkFramework;
using VContainer;

public class VContainerTransientRegisterScenario : Scenario
{
    public override string Name => "VContainer";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientContainerRegistrator.Register(builder);
    }
}