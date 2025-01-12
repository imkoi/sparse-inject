#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexTransientFirstResolve_Depth4Scenario : Scenario
{
    public override string Name => "Reflex";
    
    private Container _container;

    public override void BeforeExecute()
    {
        var builder = new ContainerBuilder();
        
        ReflexTransientRegistrator_Depth4.Register(builder);
        
        _container = builder.Build();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth4>();
    }
}
#endif