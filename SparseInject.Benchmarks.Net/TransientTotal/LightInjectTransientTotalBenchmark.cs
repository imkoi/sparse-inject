using SparseInject.Benchmarks.Core;

public class LightInjectUncompiledTransientTotalBenchmark : Benchmark
{
    public override string Name => "LightInject";

    public override void Execute()
    {
        var container = new LightInject.ServiceContainer();
        
        LightInjectTransientContainerRegistrator.Register(container);
        
        container.GetInstance(typeof(Class0));
    }
}