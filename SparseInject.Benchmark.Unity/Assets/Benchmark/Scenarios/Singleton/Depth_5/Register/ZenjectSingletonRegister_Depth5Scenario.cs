#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Zenject;

public class ZenjectSingletonRegister_Depth5Scenario : Scenario
{
    public override string Name => "Zenject";
    
    public override void Execute()
    {
        var container = new DiContainer();
        
        ZenjectSingletonRegistrator_Depth5.Register(container);
    }
}
#endif