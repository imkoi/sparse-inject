using SparseInject.BenchmarkFramework;

public class ManualTransientTotal_Depth6Scenario : Scenario
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        ManualResolver_Depth6.CreateDependency();
    }
}