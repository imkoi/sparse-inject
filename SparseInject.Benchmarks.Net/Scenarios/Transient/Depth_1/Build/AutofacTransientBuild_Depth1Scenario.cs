using Autofac;
using SparseInject.BenchmarkFramework;

public class AutofacTransientBuild_Depth1Scenario : Scenario
{
    public override string Name => "Autofac";
    
    private ContainerBuilder _builder;
    
    public override void BeforeExecute()
    {
        _builder = new ContainerBuilder();
        
        AutofacTransientRegistrator_Depth1.Register(_builder);
    }

    public override void Execute()
    {
        _builder.Build();
    }
}