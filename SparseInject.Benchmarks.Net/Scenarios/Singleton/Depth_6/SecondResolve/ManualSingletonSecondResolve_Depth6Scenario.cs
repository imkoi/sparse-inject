using SparseInject.BenchmarkFramework;

public class ManualSingletonSecondResolve_Depth6Scenario : Scenario
{
    public override string Name => "ManualResolver";

    public override void BeforeExecute()
    {
        ManualResolver_Depth6.CreateDependency();
    }

    public override void Execute()
    {
        ManualResolver_Depth6.CreateDependency();
    }
}