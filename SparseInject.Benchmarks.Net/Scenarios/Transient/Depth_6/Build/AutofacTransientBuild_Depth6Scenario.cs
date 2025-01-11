using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientBuild_Depth6Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth6.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}