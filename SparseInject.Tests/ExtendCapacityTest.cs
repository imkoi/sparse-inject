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
    public void ArrayFill_StartFromWithBatch_SlotsExtendedCorrectly()
    {
        // 17
        var nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        ArrayUtilities.Fill(nums, -1, 0);
        nums.Count(num => num == -1).Should().Be(nums.Length);
        
        nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        ArrayUtilities.Fill(nums, -1, 1);
        nums.Count(num => num == -1).Should().Be(nums.Length - 1);
        
        nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
        ArrayUtilities.Fill(nums, -1, 17);
        nums.Count(num => num == -1).Should().Be(0);
        
        // 32
        nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
            17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };
        ArrayUtilities.Fill(nums, -1, 0);
        nums.Count(num => num == -1).Should().Be(nums.Length);
        
        nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
            17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };
        ArrayUtilities.Fill(nums, -1, 16);
        nums.Count(num => num == -1).Should().Be(nums.Length - 16);
        
        nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
            17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32 };
        ArrayUtilities.Fill(nums, -1, 32);
        nums.Count(num => num == -1).Should().Be(0);
        
        // 37
        nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
            17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37 };
        ArrayUtilities.Fill(nums, -1, 0);
        nums.Count(num => num == -1).Should().Be(nums.Length);
        
        nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
            17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37 };
        ArrayUtilities.Fill(nums, -1, 7);
        nums.Count(num => num == -1).Should().Be(nums.Length - 7);
        
        nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
            17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37 };
        ArrayUtilities.Fill(nums, -1, 37);
        nums.Count(num => num == -1).Should().Be(0);
        
        nums = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16,
            17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37 };
        ArrayUtilities.Fill(nums, -1, 36);
        nums.Count(num => num == -1).Should().Be(1);
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