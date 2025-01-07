using SparseInject;
using SparseInject.Benchmarks.Core;

public class SparseInjectTransientRegisterAndBuildBenchmark : Benchmark
{
    public override string Name => "SparseInject";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectTransientContainerRegistrator.Register(builder);

        builder.Build();
    }
}