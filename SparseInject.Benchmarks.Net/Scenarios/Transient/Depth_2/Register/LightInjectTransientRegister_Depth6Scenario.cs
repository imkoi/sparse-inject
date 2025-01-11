using SparseInject.BenchmarkFramework;

public class LightInjectTransientRegister_Depth2Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectTransientRegistrator_Depth2.Register(builder);
    }
}
