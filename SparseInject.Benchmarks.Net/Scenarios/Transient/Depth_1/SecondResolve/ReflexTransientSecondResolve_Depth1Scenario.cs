#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexTransientSecondResolve_Depth1Scenario : Scenario
{
    public override string Name => "Reflex";
    
    private Container _container;

    public override void BeforeExecute()
    {
        var containerBuilder = new ContainerBuilder();
        
        ReflexTransientRegistrator_Depth1.Register(containerBuilder);
        
        _container = containerBuilder.Build();
        
        _container.Resolve<Dependency_Depth1>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth1>();
    }
}
#endif