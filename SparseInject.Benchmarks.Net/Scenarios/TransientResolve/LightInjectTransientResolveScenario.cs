using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectTransientResolveScenario : Scenario
{
    public override string Name => "LightInject";
    
    private LightInject.ServiceContainer _container;

    public override void BeforeExecute()
    {
        _container = new ServiceContainer();
        
        LightInjectTransientContainerRegistrator.Register(_container);
    }

    public override void Execute()
    {
        _container.GetInstance(typeof(Class0));
    }
}