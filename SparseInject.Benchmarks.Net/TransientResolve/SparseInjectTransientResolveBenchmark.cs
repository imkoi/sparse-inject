using SparseInject;
using SparseInject.Benchmarks.Core;

public class SparseInjectTransientResolveBenchmark : Benchmark
{
    public override string Name => "SparseInject";
    
    private Container _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        SparseInjectTransientContainerRegistrator.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve<Class0>();
    }
}