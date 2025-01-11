using SparseInject.BenchmarkFramework;

public class LightInjectTransientRegisterAndBuild_Depth6Scenario : Scenario
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectTransientRegistrator_Depth6.Register(builder);

        builder.Compile();
    }
}