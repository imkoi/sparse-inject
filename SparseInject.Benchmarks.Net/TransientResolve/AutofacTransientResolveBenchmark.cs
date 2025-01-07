using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientResolveBenchmark : Benchmark
{
    public override string Name => "Autofac";
    
    private IContainer _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        AutofacTransientContainerRegistrator.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve<Class0>();
    }
}