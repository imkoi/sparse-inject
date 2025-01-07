using SparseInject.BenchmarkFramework;

public class LightInjectTransientTotalBenchmark : Benchmark
{
    public override string Name => "LightInject";

    public override void Execute()
    {
        var container = new LightInject.ServiceContainer();
        
        LightInjectTransientContainerRegistrator.Register(container);
        
        container.GetInstance(typeof(Class0));
    }
}