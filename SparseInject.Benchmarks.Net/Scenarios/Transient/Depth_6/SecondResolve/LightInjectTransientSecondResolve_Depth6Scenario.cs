#if NET
using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectTransientSecondResolve_Depth6Scenario : Scenario
{
    public override string Name => "LightInject";
    
    private ServiceContainer _container;

    public override void BeforeExecute()
    {
        _container = new ServiceContainer();
        
        LightInjectTransientRegistrator_Depth6.Register(_container);
        
        _container.GetInstance(typeof(Dependency_Depth6));
    }

    public override void Execute()
    {
        _container.GetInstance(typeof(Dependency_Depth6));
    }
}
#endif
