using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

public class ExtendCapacityTest
{
    private class Dependency : IDisposable
    {
        public void Dispose()
        {
            
        }
    }
    
    private class DisposeCounter : IDisposable
    {
        public int Calls { get; private set; }
        
        public void Dispose()
        {
            Calls++;
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
    
    [Test]
    public void SingletonsDoubleRegisterMarkedAsDisposable_WhenInstancesCreated_ResizeAndDisposeIsCalled()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();

        var container = builder.Build();
        
        var interfaceSingletons = container.Resolve<IDisposable[]>();

        foreach (var singleton in interfaceSingletons)
        {
            if (singleton is DisposeCounter disposeCounter)
            {
                disposeCounter.Calls.Should().Be(0);
            }
        }

        container.Dispose();
        
        // Asserts
        foreach (var singleton in interfaceSingletons)
        {
            if (singleton is DisposeCounter disposeCounter)
            {
                disposeCounter.Calls.Should().Be(1);
            }
        }
    }
}