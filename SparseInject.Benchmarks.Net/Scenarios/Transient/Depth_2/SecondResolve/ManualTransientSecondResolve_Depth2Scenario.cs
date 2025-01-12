using SparseInject.BenchmarkFramework;

public class ManualTransientSecondResolve_Depth2Scenario : Scenario
{
    public override string Name => "ManualResolver";

    public override void BeforeExecute()
    {
        ManualResolver_Depth2.CreateDependency();
    }

    public override void Execute()
    {
        ManualResolver_Depth2.CreateDependency();
    }
}