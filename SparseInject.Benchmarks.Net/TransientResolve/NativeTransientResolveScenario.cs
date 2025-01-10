using SparseInject.BenchmarkFramework;

public class NativeTransientResolveScenario : Scenario
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        NativeResolver.CreateClass0();
    }
}