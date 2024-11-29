using FluentAssertions;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class CircularDependencyTest
{
    private class ServiceA
    {
        public ServiceA(ServiceB serviceB)
        {
            
        }
    }

    private class ServiceB
    {
        public ServiceB(ServiceA serviceA)
        {
            
        }
    }
  
    [Test]
    public void TransientBinding()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<ServiceA>();
        containerBuilder.Register<ServiceB>();

        containerBuilder.Invoking(subject => subject.Build()).Should().Throw<SparseInjectException>();
    }
}