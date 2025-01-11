using SparseInject.BenchmarkFramework;

public class ManualSingletonFirstResolve_Depth1Scenario : Scenario
{
    public override string Name => "ManualResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth1.CreateDependency();
    }
}