#if NET
using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectTransientFirstResolve_Depth1Scenario : Scenario
{
    public override string Name => "LightInject";
    
    private ServiceContainer _container;

    public override void BeforeExecute()
    {
        _container = new ServiceContainer();
        
        LightInjectTransientRegistrator_Depth1.Register(_container);
    }

    public override void Execute()
    {
        _container.GetInstance(typeof(Dependency_Depth1));
    }
}
#endif
