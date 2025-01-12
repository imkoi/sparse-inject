#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Zenject;

public class ZenjectTransientRegisterAndBuild_Depth6Scenario : Scenario
{
    public override string Name => "Zenject";
    
    public override void Execute()
    {
        var container = new DiContainer();
        
        ZenjectTransientRegistrator_Depth6.Register(container);

        container.ResolveRoots();
    }
}
#endif