using SparseInject.BenchmarkFramework;

public class LightInjectSingletonRegister_Depth4Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth4.Register(builder);
    }
}
