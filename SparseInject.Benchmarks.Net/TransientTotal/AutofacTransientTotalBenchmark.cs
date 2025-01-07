using Autofac;
using SparseInject.Benchmarks.Core;

public class AutofacTransientTotalBenchmark : Benchmark
{
    public override string Name => "Autofac";

    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientContainerRegistrator.Register(builder);
        
        var container = builder.Build();
        
        container.Resolve<Class0>();
    }
}