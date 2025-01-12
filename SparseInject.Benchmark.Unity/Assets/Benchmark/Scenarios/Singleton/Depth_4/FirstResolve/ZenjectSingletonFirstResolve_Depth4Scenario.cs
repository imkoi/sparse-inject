#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Zenject;

public class ZenjectSingletonFirstResolve_Depth4Scenario : Scenario
{
    public override string Name => "Zenject";
    
    private DiContainer _container;

    public override void BeforeExecute()
    {
        _container = new DiContainer();
        
        ZenjectSingletonRegistrator_Depth4.Register(_container);
        
        _container.ResolveRoots();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth4>();
    }
}
#endif