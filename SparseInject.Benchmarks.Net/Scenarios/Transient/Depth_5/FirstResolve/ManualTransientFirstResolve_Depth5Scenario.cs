using SparseInject.BenchmarkFramework;

public class ManualTransientFirstResolve_Depth5Scenario : Scenario
{
    public override string Name => "ManualResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth5.CreateDependency();
    }
}