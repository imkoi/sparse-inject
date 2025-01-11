using SparseInject.BenchmarkFramework;

public class NativeTransientTotalScenario : Scenario
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        NativeResolver.CreateClass0();
    }
}