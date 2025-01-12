#if NET
using SparseInject.BenchmarkFramework;

public class LightInjectTransientRegisterAndBuild_Depth1Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectTransientRegistrator_Depth1.Register(builder);

        builder.Compile();
    }
}
#endif
