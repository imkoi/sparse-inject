#if NET
using SparseInject.BenchmarkFramework;

public class LightInjectSingletonRegisterAndBuild_Depth3Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth3.Register(builder);

        builder.Compile();
    }
}
#endif
