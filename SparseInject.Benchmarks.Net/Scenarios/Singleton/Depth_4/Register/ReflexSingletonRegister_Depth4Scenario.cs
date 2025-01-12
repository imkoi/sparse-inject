#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexSingletonRegister_Depth4Scenario : Scenario
{
    public override string Name => "Reflex";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        ReflexSingletonRegistrator_Depth4.Register(builder);
    }
}
#endif