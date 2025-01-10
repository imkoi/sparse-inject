using SparseInject.BenchmarkFramework;

public class LightInjectTransientRegisterScenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectTransientContainerRegistrator.Register(builder);
    }
}
