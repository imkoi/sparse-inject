using SparseInject.BenchmarkFramework;

public class ManualTransientTotal_Depth2Scenario : Scenario
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth2.CreateDependency();
    }
}