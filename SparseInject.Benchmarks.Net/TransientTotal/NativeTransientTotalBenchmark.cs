using SparseInject.BenchmarkFramework;

public class NativeTransientTotalBenchmark : Benchmark
{
    public override string Name => "NativeResolver";
    
    public override void Execute()
    {
        NativeResolver.CreateClass0();
    }
}