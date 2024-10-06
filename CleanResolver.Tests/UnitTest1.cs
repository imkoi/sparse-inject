using System;
using CleanResolver.Tests.TestSources;
using FluentAssertions;
using NUnit.Framework;

namespace CleanResolver.Tests
{
    public class Tests
    {
        private ContainerBuilder _containerBuilder;
    
        [SetUp]
        public void Setup()
        {
            _containerBuilder = new ContainerBuilder();
        }

        [Test]
        public void ManyBindings()
        {
            ContainerBinder.BindDeps(_containerBuilder);

            var container = _containerBuilder.Build();
        
            var highestDependency = container.Resolve<Class0>();
        
            highestDependency.Should().NotBeNull();
        }
    }
}