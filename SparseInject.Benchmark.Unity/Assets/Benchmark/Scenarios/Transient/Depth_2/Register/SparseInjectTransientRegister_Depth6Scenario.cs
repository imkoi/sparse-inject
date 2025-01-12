using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientRegister_Depth2Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth2.Register(builder);
    }
}