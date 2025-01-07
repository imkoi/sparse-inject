using Autofac;
using SparseInject.Benchmarks.Core;

public class AutofacTransientRegisterAndBuildBenchmark : Benchmark
{
    public override string Name => "Autofac";
    
    public override void Execute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientContainerRegistrator.Register(builder);

        builder.Build();
    }
}