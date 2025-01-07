using SparseInject.BenchmarkFramework;

public class LightInjectTransientRegisterAndBuildBenchmark : Benchmark
{
    public override string Name => "LightInject";
    
    public override void Execute()
    {
        var builder = new LightInject.ServiceContainer();
        
        LightInjectTransientContainerRegistrator.Register(builder);

        builder.Compile();
    }
}