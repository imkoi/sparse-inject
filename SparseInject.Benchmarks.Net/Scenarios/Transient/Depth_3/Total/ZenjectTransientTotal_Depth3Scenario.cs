#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Zenject;

public class ZenjectTransientTotal_Depth3Scenario : Scenario
{
    public override string Name => "Zenject";

    public override void Execute()
    {
        var container = new DiContainer();
        
        ZenjectTransientRegistrator_Depth3.Register(container);
        
        container.ResolveRoots();
        
        container.Resolve<Dependency_Depth3>();
    }
}
#endif