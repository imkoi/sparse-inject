using SparseInject;
using SparseInject.BenchmarkFramework;

public class SparseInjectTransientBuildBenchmark : Benchmark
{
    public override string Name => "SparseInject";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        SparseInjectTransientContainerRegistrator.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}