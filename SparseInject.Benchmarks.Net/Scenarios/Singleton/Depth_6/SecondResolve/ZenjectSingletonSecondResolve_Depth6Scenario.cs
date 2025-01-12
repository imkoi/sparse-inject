#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Zenject;

public class ZenjectSingletonSecondResolve_Depth6Scenario : Scenario
{
    public override string Name => "Zenject";
    
    private DiContainer _container;

    public override void BeforeExecute()
    {
        _container = new DiContainer();
        
        ZenjectSingletonRegistrator_Depth6.Register(_container);
        
        _container.ResolveRoots();
        
        _container.Resolve<Dependency_Depth6>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth6>();
    }
}
#endif