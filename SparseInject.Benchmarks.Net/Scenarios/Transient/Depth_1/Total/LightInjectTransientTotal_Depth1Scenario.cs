using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectTransientTotal_Depth1Scenario : Scenario
{
    public override string Name => "LightInject";

    public override void Execute()
    {
        var container = new ServiceContainer();
        
        LightInjectTransientRegistrator_Depth1.Register(container);
        
        container.GetInstance(typeof(Dependency_Depth1));
    }
}