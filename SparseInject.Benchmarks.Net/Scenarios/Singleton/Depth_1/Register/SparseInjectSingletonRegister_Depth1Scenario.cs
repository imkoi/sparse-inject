using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonRegister_Depth1Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth1.Register(builder);
    }
}