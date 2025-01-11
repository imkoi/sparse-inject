using SparseInject.BenchmarkFramework;

public class LightInjectSingletonRegister_Depth1Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth1.Register(builder);
    }
}
