using SparseInject.BenchmarkFramework;

public class ManualTransientFirstResolve_Depth4Scenario : Scenario
{
    public override string Name => "ManualResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth4.CreateDependency();
    }
}