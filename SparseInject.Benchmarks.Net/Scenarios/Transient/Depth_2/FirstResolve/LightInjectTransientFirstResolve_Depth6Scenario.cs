#if NET
using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectTransientFirstResolve_Depth2Scenario : Scenario
{
    public override string Name => "LightInject";
    
    private ServiceContainer _container;

    public override void BeforeExecute()
    {
        _container = new ServiceContainer();
        
        LightInjectTransientRegistrator_Depth2.Register(_container);
    }

    public override void Execute()
    {
        _container.GetInstance(typeof(Dependency_Depth2));
    }
}
#endif
