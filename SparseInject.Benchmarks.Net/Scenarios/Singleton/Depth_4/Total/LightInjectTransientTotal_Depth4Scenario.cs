using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectSingletonTotal_Depth4Scenario : Scenario
{
    public override string Name => "LightInject";

    public override void Execute()
    {
        var container = new ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth4.Register(container);
        
        container.GetInstance(typeof(Dependency_Depth4));
    }
}