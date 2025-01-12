using SparseInject.BenchmarkFramework;

public class ManualSingletonTotal_Depth6Scenario : Scenario
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth6.CreateDependency();
    }
}