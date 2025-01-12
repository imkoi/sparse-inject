using SparseInject.BenchmarkFramework;

public class ManualTransientTotal_Depth3Scenario : Scenario
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth3.CreateDependency();
    }
}