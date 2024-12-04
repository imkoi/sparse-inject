using System;
using FluentAssertions;
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

    private class TypeWithNotRegisteredCollectionDependency
    {
        public IDisposable[] Collection { get; }

        public TypeWithNotRegisteredCollectionDependency(IDisposable[] collection)
        {
            Collection = collection;
        }
    }
    
    [Test]
    public void TypeWithNotRegisteredCollectionDependency_WhenResolved_HasEmptyCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<TypeWithNotRegisteredCollectionDependency>();
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<TypeWithNotRegisteredCollectionDependency>();

        instance.Collection.Should().BeEmpty();
    }
}