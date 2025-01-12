#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Zenject;

public class ZenjectTransientSecondResolve_Depth5Scenario : Scenario
{
    public override string Name => "Zenject";
    
    private DiContainer _container;

    public override void BeforeExecute()
    {
        _container = new DiContainer();
        
        ZenjectTransientRegistrator_Depth5.Register(_container);
        
        _container.ResolveRoots();
        
        _container.Resolve<Dependency_Depth5>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth5>();
    }
}
#endif