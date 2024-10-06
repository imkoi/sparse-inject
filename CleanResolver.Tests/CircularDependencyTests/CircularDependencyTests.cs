using FluentAssertions;
using NUnit.Framework;

namespace CleanResolver.Tests.CircularDependency
{
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
            
            var container = _containerBuilder.Build();
            
            var processors = container.Resolve<ServiceA[]>();
        }
    }
}