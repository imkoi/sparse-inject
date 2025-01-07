using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientRegisterBenchmark : Benchmark
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectTransientContainerRegistrator.Register(builder);
    }
}