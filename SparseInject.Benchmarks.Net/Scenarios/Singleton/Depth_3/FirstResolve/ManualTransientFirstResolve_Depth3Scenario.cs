using SparseInject.BenchmarkFramework;

public class ManualSingletonFirstResolve_Depth3Scenario : Scenario
{
    public override string Name => "ManualResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth3.CreateDependency();
    }
}