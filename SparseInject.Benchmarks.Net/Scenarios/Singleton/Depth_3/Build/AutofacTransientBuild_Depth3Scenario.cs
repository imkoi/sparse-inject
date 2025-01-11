using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacSingletonBuild_Depth3Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        AutofacSingletonRegistrator_Depth3.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}