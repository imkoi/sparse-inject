using SparseInject.Tests.CircularDependency;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class CircularDependencyTests
{
    private ContainerBuilder _containerBuilder;
    
    [SetUp]
    public void Setup()
    {
        _containerBuilder = new ContainerBuilder();
    }
        
    [Test]
    public void TransientBinding()
    {
        _containerBuilder.Register<ServiceA>();
        _containerBuilder.Register<ServiceB>();

        Assert.Throws<SparseInjectException>(() => _containerBuilder.Build());
    }
}