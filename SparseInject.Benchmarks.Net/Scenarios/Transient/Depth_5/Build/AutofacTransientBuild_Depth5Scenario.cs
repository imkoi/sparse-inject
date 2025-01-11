using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientBuild_Depth5Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth5.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}