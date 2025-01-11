using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonBuild_Depth4Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth4.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}