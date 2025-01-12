#if NET
using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectSingletonTotal_Depth3Scenario : Scenario
{
    public override string Name => "LightInject";

    public override void Execute()
    {
        var container = new ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth3.Register(container);
        
        container.GetInstance(typeof(Dependency_Depth3));
    }
}
#endif
