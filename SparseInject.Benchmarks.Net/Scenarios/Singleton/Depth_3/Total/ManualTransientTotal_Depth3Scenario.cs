using SparseInject.BenchmarkFramework;

public class ManualSingletonTotal_Depth3Scenario : Scenario
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth3.CreateDependency();
    }
}