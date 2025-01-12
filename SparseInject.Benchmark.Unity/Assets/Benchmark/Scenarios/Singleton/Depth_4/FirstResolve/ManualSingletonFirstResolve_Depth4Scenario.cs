using SparseInject.BenchmarkFramework;

public class ManualSingletonFirstResolve_Depth4Scenario : Scenario
{
    public override string Name => "ManualResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth4.CreateDependency();
    }
}