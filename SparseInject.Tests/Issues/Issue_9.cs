using FluentAssertions;
using NUnit.Framework;
using Test;

namespace SparseInject.Tests.Issues
{
    public class Issue_9
    {
        [Test]
        public void Test()
        {
            // Factory was generated
            ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(TestService), out var factory, out _)
                .Should().BeTrue();
            factory.Should().NotBeNull();
            
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Register<TestService>();
            var container = containerBuilder.Build();
            
            // instance could be resolved from factory
            container.Resolve<TestService>().Should().NotBeNull();
        }
    }
}