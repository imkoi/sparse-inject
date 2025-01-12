#if UNITY_2017_1_OR_NEWER
using SparseInject.BenchmarkFramework;
using Reflex.Core;

public class ReflexSingletonSecondResolve_Depth2Scenario : Scenario
{
    public override string Name => "Reflex";
    
    private Container _container;

    public override void BeforeExecute()
    {
        var containerBuilder = new ContainerBuilder();
        
        ReflexSingletonRegistrator_Depth2.Register(containerBuilder);
        
        _container = containerBuilder.Build();
        
        _container.Resolve<Dependency_Depth2>();
    }

    public override void Execute()
    {
        _container.Resolve<Dependency_Depth2>();
    }
}
#endif