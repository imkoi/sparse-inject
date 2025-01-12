#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Zenject;

public class ZenjectTransientRegister_Depth1Scenario : Scenario
{
    public override string Name => "Zenject";
    
    public override void Execute()
    {
        var container = new DiContainer();
        
        ZenjectTransientRegistrator_Depth1.Register(container);
    }
}
#endif