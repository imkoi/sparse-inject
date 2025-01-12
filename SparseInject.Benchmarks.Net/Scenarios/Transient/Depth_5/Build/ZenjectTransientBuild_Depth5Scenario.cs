#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Zenject;

public class ZenjectTransientBuild_Depth5Scenario : Scenario
{
    public override string Name => "Zenject";
    
    private DiContainer _container;
    
    public override void BeforeExecute()
    {
        _container = new DiContainer();
        
        ZenjectTransientRegistrator_Depth5.Register(_container);
    }

    public override void Execute()
    {
        _container.ResolveRoots();
    }
}
#endif