using SparseInject.BenchmarkFramework;

public class ManualTransientFirstResolve_Depth1Scenario : Scenario
{
    public override string Name => "ManualResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth1.CreateDependency();
    }
}