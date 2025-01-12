using SparseInject.BenchmarkFramework;

public class ManualSingletonFirstResolve_Depth6Scenario : Scenario
{
    public override string Name => "ManualResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth6.CreateDependency();
    }
}