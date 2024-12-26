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
    
    [Ignore("Need fixes in ContainerBuilder")]
    [Test]
    public void RegisterTwoCollections_WhenResolveJaggedCollection_ReturnConcatenatedCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[2]
        {
            new DependencyA(),
            new DependencyA()
        });
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyB[2]
        {
            new DependencyB(),
            new DependencyB()
        });
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instances = container.Resolve<IDisposable[][]>();
    
        instances.Length.Should().Be(2);
        
        var collectionA = instances[0];
        var collectionB = instances[1];
        
        collectionA.Length.Should().Be(2);
        collectionB.Length.Should().Be(2);
        
        collectionA[0].Should().BeOfType<DependencyA>();
        collectionA[1].Should().BeOfType<DependencyA>();
        
        collectionB[0].Should().BeOfType<DependencyB>();
        collectionB[0].Should().BeOfType<DependencyB>();
    }
    
    // TODO: create more test cases
}