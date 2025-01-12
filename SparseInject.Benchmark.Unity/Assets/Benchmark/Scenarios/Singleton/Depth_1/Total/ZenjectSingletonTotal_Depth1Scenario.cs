#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Zenject;

public class ZenjectSingletonTotal_Depth1Scenario : Scenario
{
    public override string Name => "Zenject";

    public override void Execute()
    {
        var container = new DiContainer();
        
        ZenjectSingletonRegistrator_Depth1.Register(container);
        
        container.ResolveRoots();
        
        container.Resolve<Dependency_Depth1>();
    }
}
#endif