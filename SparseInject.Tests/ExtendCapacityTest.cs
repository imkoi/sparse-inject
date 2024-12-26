using System;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class ExtendCapacityTest
{
    private class Dependency : IDisposable
    {
        public void Dispose()
        {
            
        }
    }
    
    [Test]
    public void RegisteredTwoDisposables_WhenNotEnoughCapacity_CapacityWasExtendedProperly()
    {
        // Setup
        var containerBuilder = new ContainerBuilder(1);
        
        containerBuilder.Register<IDisposable, Dependency>();
        containerBuilder.Register<IDisposable, Dependency>();
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<IDisposable>();

        instance.Should().BeOfType<Dependency>();
        
        var instances = container.Resolve<IDisposable[]>();
        
        instances.Length.Should().Be(2);
    }
    
    [Test]
    public void ArrayCacheReserved_WhenNotEnoughSlots_SlotsExtendedCorrectly()
    {
        ArrayCache.PullReserved(2048).Array.Length.Should().BeGreaterThan(2048);

        ArrayCache.PushReserved(2048);
        
        ArrayCache.PullReserved(2048).Array.Length.Should().BeGreaterThan(2048);
        
        ArrayCache.PushReserved(2048);
    }
}