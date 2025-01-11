using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectTransientTotal_Depth6Scenario : Scenario
{
    public override string Name => "LightInject";

    public override void Execute()
    {
        var container = new ServiceContainer();
        
        LightInjectTransientRegistrator_Depth6.Register(container);
        
        container.GetInstance(typeof(Dependency_Depth6));
    }
}