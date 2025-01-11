using SparseInject.BenchmarkFramework;

public class LightInjectSingletonRegister_Depth2Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth2.Register(builder);
    }
}
