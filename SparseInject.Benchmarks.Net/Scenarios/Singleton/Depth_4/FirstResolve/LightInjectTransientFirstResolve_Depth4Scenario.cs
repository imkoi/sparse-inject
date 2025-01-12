#if NET
using LightInject;
using SparseInject.BenchmarkFramework;

public class LightInjectSingletonFirstResolve_Depth4Scenario : Scenario
{
    public override string Name => "LightInject";
    
    private ServiceContainer _container;

    public override void BeforeExecute()
    {
        _container = new ServiceContainer();
        
        LightInjectSingletonRegistrator_Depth4.Register(_container);
    }

    public override void Execute()
    {
        _container.GetInstance(typeof(Dependency_Depth4));
    }
}
#endif
