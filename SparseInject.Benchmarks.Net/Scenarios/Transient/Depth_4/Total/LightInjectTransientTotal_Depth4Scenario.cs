#if NET
using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectTransientTotal_Depth4Scenario : Scenario
{
    public override string Name => "LightInject";

    public override void Execute()
    {
        var container = new ServiceContainer();
        
        LightInjectTransientRegistrator_Depth4.Register(container);
        
        container.GetInstance(typeof(Dependency_Depth4));
    }
}
#endif
