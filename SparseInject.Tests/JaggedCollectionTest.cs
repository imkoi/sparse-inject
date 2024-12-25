using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class JaggedCollectionTest
{
    private class DependencyA : IDisposable
    {
        public void Dispose()
        {
            
        }
    }
    
    private class DependencyB : IDisposable
    {
        public void Dispose()
        {
            
        }
    }
    
    [Test]
    public void UnregisteredJaggedCollection_WhenResolved_ReturnEmptyCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        var container = containerBuilder.Build();

        // Asserts
        var disposableCollection = container.Resolve<IDisposable[][]>();

        disposableCollection.Should().BeEmpty();
    }
    
    [Test]
    public void RegisterEmptyJaggedCollection_WhenResolveCollection_ReturnEmptyCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[][]>(new DependencyA[0][]);
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instances = container.Resolve<IDisposable[][]>();
    
        instances.Length.Should().Be(0);
    }
    
    [Test]
    public void RegisterJaggedCollectionWithOneElement_WhenResolveCollection_ReturnCorrectCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[][]>(new DependencyA[1][]
        {
            new DependencyA[1]
            {
                new DependencyA()
            }
        });
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instances = container.Resolve<IDisposable[][]>();
    
        instances.Length.Should().Be(1);
        instances[0].Should().BeOfType<DependencyA[]>();
        instances[0][0].Should().BeOfType<DependencyA>();
    }
    
    [Test]
    public void RegisterJaggedCollectionWithOneElement_WhenResolveRankZeroCollection_ReturnEmptyCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[][]>(new DependencyA[1][]
        {
            new DependencyA[1]
            {
                new DependencyA()
            }
        });
        
        var container = containerBuilder.Build();

        // Asserts
        var instances = container.Resolve<IDisposable[]>();
        
        instances.Length.Should().Be(0);
    }
    
    [Test]
    public void RegisterJaggedAndSimpleCollection_WhenResolveCollection_ReturnNotConcatenatedCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[2]
        {
            new DependencyA(),
            new DependencyA()
        });
        containerBuilder.RegisterValue<IDisposable[][]>(new DependencyA[2][]
        {
            new DependencyA[2]
            {
                new DependencyA(),
                new DependencyA()
            },
            new DependencyA[2]
            {
                new DependencyA(),
                new DependencyA()
            }
        });
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instances = container.Resolve<IDisposable[]>();
    
        instances.Length.Should().Be(2);
        instances[0].Should().BeOfType<DependencyA>();
        instances[1].Should().BeOfType<DependencyA>();
    }
    
    // TODO: create more test cases
}