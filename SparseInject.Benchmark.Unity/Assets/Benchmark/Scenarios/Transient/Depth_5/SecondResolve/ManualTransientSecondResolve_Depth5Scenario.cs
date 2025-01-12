using SparseInject.BenchmarkFramework;

public class ManualTransientSecondResolve_Depth5Scenario : Scenario
{
    public override string Name => "ManualResolver";

    public override void BeforeExecute()
    {
        ManualResolver_Depth5.CreateDependency();
    }

    public override void Execute()
    {
        ManualResolver_Depth5.CreateDependency();
    }
}