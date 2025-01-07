using SparseInject.BenchmarkFramework;

public class NativeTransientResolveBenchmark : Benchmark
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        NativeResolver.CreateClass0();
    }
}