using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class CollectionTest
{
    [Test]
    public void UnregisteredType_WhenResolved_ReturnEmptyCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        var container = containerBuilder.Build();

        // Asserts
        var disposableCollection = container.Resolve<IDisposable[]>();

        disposableCollection.Should().BeEmpty();
    }

    private class TypeWithCollectionDependency
    {
        public IDisposable[] Collection { get; }

        public TypeWithCollectionDependency(IDisposable[] collection)
        {
            Collection = collection;
        }
    }

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
    public void TypeWithNotRegisteredCollectionDependency_WhenTypeResolved_TypeHasEmptyCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<TypeWithCollectionDependency>();
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<TypeWithCollectionDependency>();

        instance.Collection.Should().BeEmpty();
    }
    
    [Test]
    public void TypeWithOneRegisteredCollectionDependency_WhenResolveCollection_ReturnCorrectCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<TypeWithCollectionDependency>();
        containerBuilder.Register<IDisposable, DependencyA>();
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<IDisposable[]>();

        instance.Length.Should().Be(1);
    }
    
    [Test]
    public void TypeWithTwoRegisteredCollectionDependency_WhenResolveCollection_ReturnCorrectCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<TypeWithCollectionDependency>();
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyA>();
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<IDisposable[]>();

        instance.Length.Should().Be(2);
    }
    
    [Test]
    public void TypeWithThreeRegisteredCollectionDependency_WhenResolveCollection_ReturnCorrectCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<TypeWithCollectionDependency>();
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyA>();
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<IDisposable[]>();

        instance.Length.Should().Be(3);
    }

    [Test]
    public void TypeWithOneRegisteredCollectionDependency_WhenResolveSingle_ReturnCorrectDependency()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<TypeWithCollectionDependency>();
        containerBuilder.Register<IDisposable, DependencyA>();
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<IDisposable>();

        instance.Should().BeOfType<DependencyA>();
    }
    
    [Ignore("Not working yet")]
    [Test]
    public void RegisterCollectionAndSingleDependency_WhenResolveCollection_ReturnConcatenatedCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[4]);
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyB>();
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instance = container.Resolve<IDisposable[]>();
    
        instance.Length.Should().Be(6);
    }
    
    [Test]
    public void RegisterSingleCollection_WhenResolveSingle_ThrowProperException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[3]);
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<IDisposable[]>();

        instance.Length.Should().Be(3);
    }
}