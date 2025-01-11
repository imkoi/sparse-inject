using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientRegister_Depth5Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectTransientRegistrator_Depth5.Register(builder);
    }
}