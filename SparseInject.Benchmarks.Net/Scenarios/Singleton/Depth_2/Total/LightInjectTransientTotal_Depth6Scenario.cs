using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectSingletonTotal_Depth2Scenario : Scenario
{
    public override string Name => "LightInject";

    public override void Execute()
    {
        var container = new ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth2.Register(container);
        
        container.GetInstance(typeof(Dependency_Depth2));
    }
}