#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Zenject;

public class ZenjectSingletonRegisterAndBuild_Depth4Scenario : Scenario
{
    public override string Name => "Zenject";
    
    public override void Execute()
    {
        var container = new DiContainer();
        
        ZenjectSingletonRegistrator_Depth4.Register(container);

        container.ResolveRoots();
    }
}
#endif