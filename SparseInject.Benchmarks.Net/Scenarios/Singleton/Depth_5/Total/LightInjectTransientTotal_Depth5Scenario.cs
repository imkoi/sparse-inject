using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectSingletonTotal_Depth5Scenario : Scenario
{
    public override string Name => "LightInject";

    public override void Execute()
    {
        var container = new ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth5.Register(container);
        
        container.GetInstance(typeof(Dependency_Depth5));
    }
}