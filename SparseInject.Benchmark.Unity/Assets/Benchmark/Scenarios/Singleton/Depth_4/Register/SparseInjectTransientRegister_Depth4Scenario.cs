using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectSingletonRegister_Depth4Scenario : Scenario
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectSingletonRegistrator_Depth4.Register(builder);
    }
}