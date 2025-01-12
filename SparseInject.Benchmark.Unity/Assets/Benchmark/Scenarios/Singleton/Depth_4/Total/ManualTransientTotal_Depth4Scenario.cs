using SparseInject.BenchmarkFramework;

public class ManualSingletonTotal_Depth4Scenario : Scenario
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth4.CreateDependency();
    }
}