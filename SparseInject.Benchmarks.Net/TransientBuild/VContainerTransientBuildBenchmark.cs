using SparseInject.Benchmarks.Core;
using VContainer;

public class VContainerTransientBuildBenchmark : Benchmark
{
    public override string Name => "VContainer";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        VContainerTransientContainerRegistrator.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}