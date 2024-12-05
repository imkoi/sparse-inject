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

    private class Dependency : IDisposable
    {
        public void Dispose()
        {
            
        }
    }
    
    [Test]
    public void TypeWithNotRegisteredCollectionDependency_WhenResolved_HasEmptyCollection()
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
    public void TypeWithOneRegisteredCollectionDependency_WhenResolved_HasCollectionWithOneItem()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<TypeWithCollectionDependency>();
        containerBuilder.Register<IDisposable, Dependency>();
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<TypeWithCollectionDependency>();

        instance.Collection.Length.Should().Be(1);
    }
}