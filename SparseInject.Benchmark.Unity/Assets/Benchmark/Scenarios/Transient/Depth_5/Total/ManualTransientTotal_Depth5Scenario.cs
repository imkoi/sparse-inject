using SparseInject.BenchmarkFramework;

public class ManualTransientTotal_Depth5Scenario : Scenario
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth5.CreateDependency();
    }
}