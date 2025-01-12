using SparseInject.BenchmarkFramework;

public class ManualSingletonSecondResolve_Depth4Scenario : Scenario
{
    public override string Name => "ManualResolver";

    public override void BeforeExecute()
    {
        ManualResolver_Depth4.CreateDependency();
    }

    public override void Execute()
    {
        ManualResolver_Depth4.CreateDependency();
    }
}