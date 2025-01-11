using SparseInject.BenchmarkFramework;

public class LightInjectTransientRegister_Depth1Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectTransientRegistrator_Depth1.Register(builder);
    }
}
