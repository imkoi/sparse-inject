#if NET
using SparseInject.BenchmarkFramework;

public class LightInjectSingletonRegister_Depth5Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth5.Register(builder);
    }
}
#endif
