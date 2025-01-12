#if NET
using SparseInject.BenchmarkFramework;

public class LightInjectTransientRegisterAndBuild_Depth4Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectTransientRegistrator_Depth4.Register(builder);

        builder.Compile();
    }
}
#endif
