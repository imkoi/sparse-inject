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
    
    [Ignore("not work yet")]
    [Test]
    public void RegisterCollectionAndSingleDependency_WhenResolveCollection_ReturnConcatenatedCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[4]);
        containerBuilder.Register<IDisposable, DependencyA>(); // should not set new index for collection - should take collection concretes index
        containerBuilder.Register<IDisposable, DependencyB>();
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instance = container.Resolve<IDisposable[]>();
    
        instance.Length.Should().Be(6);
    }
    
    [Test]
    public void RegisterDependenciesAndCollection_WhenResolveCollection_ReturnConcatenatedCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyB>();
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[4] {new DependencyA(), new DependencyA(), new DependencyA(), new DependencyA()});
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instances = container.Resolve<IDisposable[]>();
    
        instances.Length.Should().Be(6);
        instances[0].Should().BeOfType<DependencyA>();
        instances[1].Should().BeOfType<DependencyB>();
        instances[2].Should().BeOfType<DependencyA>();
        instances[3].Should().BeOfType<DependencyA>();
        instances[4].Should().BeOfType<DependencyA>();
        instances[5].Should().BeOfType<DependencyA>();
    }
    
    [Test]
    public void RegisterDependenciesAndCollectionWithOneElement_WhenResolveCollection_ReturnConcatenatedCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyB>();
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[1] { new DependencyA() });
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instances = container.Resolve<IDisposable[]>();
    
        instances.Length.Should().Be(3);
        instances[0].Should().BeOfType<DependencyA>();
        instances[1].Should().BeOfType<DependencyB>();
        instances[2].Should().BeOfType<DependencyA>();
    }
    
    [Test]
    public void RegisterDependenciesAndEmptyCollection_WhenResolveCollection_ReturnConcatenatedCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyB>();
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[0]);
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instances = container.Resolve<IDisposable[]>();
    
        instances.Length.Should().Be(2);
        instances[0].Should().BeOfType<DependencyA>();
        instances[1].Should().BeOfType<DependencyB>();
    }
    
    [Test]
    public void RegisterDependenciesAndCollection_WhenResolveSingle_ReturnCorrectInstance()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyB>();
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[2]);
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instance = container.Resolve<IDisposable>();
    
        instance.Should().BeOfType<DependencyB>();
    }
    
    [Test]
    public void RegisterCollectionAsValue_WhenResolve_ReturnCollectionWithProperElementsCount()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[3]);
        
        var container = containerBuilder.Build();

        // Asserts
        var instances = container.Resolve<IDisposable[]>();

        instances.Length.Should().Be(3);
    }
    
    [Test]
    public void RegisterTwoCollectionsAsValue_WhenResolve_ReturnConcatenatedCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[3]);
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[3]);
        
        var container = containerBuilder.Build();

        // Asserts
        var instances = container.Resolve<IDisposable[]>();

        instances.Length.Should().Be(6);
    }
    
    [Test]
    public void RegisterCollectionsAsValue_WhenResolveElement_ThrowProperException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[3]);
        
        var container = containerBuilder.Build();

        // Asserts
        container
            .Invoking(subject => subject.Resolve<IDisposable>()).Should()
            .Throw<SparseInjectException>();
    }
}