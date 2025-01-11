using SparseInject.BenchmarkFramework;

public class ManualTransientSecondResolve_Depth1Scenario : Scenario
{
    public override string Name => "ManualResolver";

    public override void BeforeExecute()
    {
        ManualResolver_Depth1.CreateDependency();
    }

    public override void Execute()
    {
        ManualResolver_Depth1.CreateDependency();
    }
}