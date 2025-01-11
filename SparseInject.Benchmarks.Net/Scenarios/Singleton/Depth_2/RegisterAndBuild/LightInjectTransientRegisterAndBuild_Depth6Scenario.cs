using SparseInject.BenchmarkFramework;

public class LightInjectSingletonRegisterAndBuild_Depth2Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth2.Register(builder);

        builder.Compile();
    }
}