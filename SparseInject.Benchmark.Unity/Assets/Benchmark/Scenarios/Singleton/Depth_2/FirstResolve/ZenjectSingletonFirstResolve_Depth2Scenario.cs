#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Zenject;

public class ZenjectSingletonFirstResolve_Depth2Scenario : Scenario
{
    public override string Name => "Zenject";
    
    private DiContainer _container;

    public override void BeforeExecute()
    {
        _container = new DiContainer();
        
        ZenjectSingletonRegistrator_Depth2.Register(_container);
        
        _container.ResolveRoots();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth2>();
    }
}
#endif