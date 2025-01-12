#if NET
using SparseInject.BenchmarkFramework;

public class LightInjectTransientRegister_Depth6Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectTransientRegistrator_Depth6.Register(builder);
    }
}
#endif
