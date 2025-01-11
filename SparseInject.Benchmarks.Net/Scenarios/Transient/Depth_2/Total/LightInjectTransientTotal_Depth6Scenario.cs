using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectTransientTotal_Depth2Scenario : Scenario
{
    public override string Name => "LightInject";

    public override void Execute()
    {
        var container = new ServiceContainer();
        
        LightInjectTransientRegistrator_Depth2.Register(container);
        
        container.GetInstance(typeof(Dependency_Depth2));
    }
}