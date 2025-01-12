using SparseInject.BenchmarkFramework;

public class ManualSingletonTotal_Depth1Scenario : Scenario
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth1.CreateDependency();
    }
}