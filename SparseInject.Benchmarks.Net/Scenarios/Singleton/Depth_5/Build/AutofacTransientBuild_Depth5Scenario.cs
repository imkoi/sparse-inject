#if NET
using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonBuild_Depth5Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth5.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}
#endif
