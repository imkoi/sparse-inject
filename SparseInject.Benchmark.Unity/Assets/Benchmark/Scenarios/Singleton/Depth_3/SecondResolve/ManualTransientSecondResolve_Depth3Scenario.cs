using SparseInject.BenchmarkFramework;

public class ManualSingletonSecondResolve_Depth3Scenario : Scenario
{
    public override string Name => "ManualResolver";

    public override void BeforeExecute()
    {
        ManualResolver_Depth3.CreateDependency();
    }

    public override void Execute()
    {
        ManualResolver_Depth3.CreateDependency();
    }
}