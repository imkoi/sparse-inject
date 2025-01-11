using SparseInject.BenchmarkFramework;

public class LightInjectTransientRegister_Depth3Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectTransientRegistrator_Depth3.Register(builder);
    }
}
