using SparseInject;
using SparseInject.Benchmarks.Core;

public class SparseInjectTransientTotalBenchmark : Benchmark
{
    public override string Name => "SparseInject";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectTransientContainerRegistrator.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve<Class0>();
    }
}