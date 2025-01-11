using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectSingletonSecondResolve_Depth1Scenario : Scenario
{
    public override string Name => "LightInject";
    
    private ServiceContainer _container;

    public override void BeforeExecute()
    {
        _container = new ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth1.Register(_container);
        
        _container.GetInstance(typeof(Dependency_Depth1));
    }

    public override void Execute()
    {
        _container.GetInstance(typeof(Dependency_Depth1));
    }
}