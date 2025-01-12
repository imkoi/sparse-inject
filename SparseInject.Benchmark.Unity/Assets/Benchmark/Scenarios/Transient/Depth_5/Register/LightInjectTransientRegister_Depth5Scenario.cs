#if NET
using SparseInject.BenchmarkFramework;

public class LightInjectTransientRegister_Depth5Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectTransientRegistrator_Depth5.Register(builder);
    }
}
#endif
