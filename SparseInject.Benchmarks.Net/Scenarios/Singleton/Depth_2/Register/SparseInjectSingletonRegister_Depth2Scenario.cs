using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonRegister_Depth2Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth2.Register(builder);
    }
}