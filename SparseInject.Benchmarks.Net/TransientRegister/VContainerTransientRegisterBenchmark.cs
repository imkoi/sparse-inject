using SparseInject.Benchmarks.Core;
using VContainer;

public class VContainerTransientRegisterBenchmark : Benchmark
{
    public override string Name => "VContainer";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        VContainerTransientContainerRegistrator.Register(builder);
    }
}