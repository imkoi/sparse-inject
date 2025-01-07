using SparseInject.Benchmarks.Core;

public class LightInjectTransientRegisterBenchmark : Benchmark
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectTransientContainerRegistrator.Register(builder);
    }
}
