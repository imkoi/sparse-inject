#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexTransientSecondResolve_Depth2Scenario : Scenario
{
    public override string Name => "Reflex";
    
    private Container _container;

    public override void BeforeExecute()
    {
        var containerBuilder = new ContainerBuilder();
        
        ReflexTransientRegistrator_Depth2.Register(containerBuilder);
        
        _container = containerBuilder.Build();
        
        _container.Resolve<Dependency_Depth2>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth2>();
    }
}
#endif