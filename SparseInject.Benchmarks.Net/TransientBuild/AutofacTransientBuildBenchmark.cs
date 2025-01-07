using Autofac;
using SparseInject.Benchmarks.Core;

public class AutofacTransientBuildBenchmark : Benchmark
{
    public override string Name => "Autofac";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        AutofacTransientContainerRegistrator.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}