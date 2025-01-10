using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SparseInject;

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
    
    [Test]
    public void RegisterCollectionAndSingleDependency_WhenResolveCollection_ReturnConcatenatedCollection()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[4]
        {
            new DependencyA(),
            new DependencyA(),
            new DependencyA(),
            new DependencyA()
        });
        containerBuilder.Register<IDisposable, DependencyA>(); // should not set new index for collection - should take collection concretes index
        containerBuilder.Register<IDisposable, DependencyB>();
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instances = container.Resolve<IDisposable[]>();

        instances[0].Should().BeOfType<DependencyA>();
        instances[1].Should().BeOfType<DependencyA>();
        instances[2].Should().BeOfType<DependencyA>();
        instances[3].Should().BeOfType<DependencyA>();
        instances[4].Should().BeOfType<DependencyA>();
        instances[5].Should().BeOfType<DependencyB>();
    }

    [Test]
    public void RegisterCollectionAndTwoSingleDependency_WhenResolveSingle_ReturnLastConcrete()
    {
        // Setup
        var containerBuilder = new ContainerBuilder(1);
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[4]
        {
            new DependencyA(),
            new DependencyA(),
            new DependencyA(),
            new DependencyA()
        });
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyB>();
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<IDisposable>();

        instance.Should().BeOfType<DependencyB>();
    }
    
    [Test]
    public void RegisterCollectionAndSingleDependency_WhenResolveSingle_ReturnCorrectConcrete()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[4]
        {
            new DependencyA(),
            new DependencyA(),
            new DependencyA(),
            new DependencyA()
        });
        containerBuilder.Register<IDisposable, DependencyA>();
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instance = container.Resolve<IDisposable>();

        instance.Should().BeOfType<DependencyA>();
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
    public void RegisterDependenciesAndCollection_WhenResolveCollectionMultipleTimes_CollectionElementsIsCorrect()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyB>();
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[4] {new DependencyA(), new DependencyA(), new DependencyA(), new DependencyA()});
        
        var container = containerBuilder.Build();
    
        // Asserts
        var instancesA = container.Resolve<IDisposable[]>();
        var instancesB = container.Resolve<IDisposable[]>();
        
        instancesA.Length.Should().Be(instancesB.Length);

        instancesA[0].Should().NotBe(instancesB[0]);
        instancesA[1].Should().NotBe(instancesB[1]);
        instancesA[2].Should().Be(instancesB[2]);
        instancesA[3].Should().Be(instancesB[3]);
        instancesA[4].Should().Be(instancesB[4]);
        instancesA[5].Should().Be(instancesB[5]);
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
    
    private class DependencyC : IDisposable { public void Dispose() { } }

    private class DependenciesCollector
    {
        public IDisposable[] Disposables { get; }

        public DependenciesCollector(IDisposable[] disposables)
        {
            Disposables = disposables;
        }
    }

    [Test]
    public void RegisterThreeDisposablesWithCollector_WhenResolve_ReturnCorrectInstances()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<DependenciesCollector>();
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyB>();
        containerBuilder.Register<IDisposable, DependencyC>();

        var container = containerBuilder.Build();
        
        // Asserts
        container.Resolve<IDisposable>().Should().BeOfType<DependencyC>();
        
        var instances = container.Resolve<IDisposable[]>();
        instances[0].Should().BeOfType<DependencyA>();
        instances[1].Should().BeOfType<DependencyB>();
        instances[2].Should().BeOfType<DependencyC>();
        
        container.Resolve<DependenciesCollector>().Disposables.Length.Should().Be(3);
    }

    [Test]
    public void RegisterThreeDisposablesUnorderedWithCollector_WhenResolve_ReturnCorrectInstances()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<IDisposable, DependencyA>();
        containerBuilder.Register<IDisposable, DependencyB>();
        containerBuilder.Register<DependenciesCollector>();
        containerBuilder.Register<IDisposable, DependencyC>();
        
        var container = containerBuilder.Build();
        
        // Asserts
        var instances = container.Resolve<IDisposable[]>();
        instances[0].Should().BeOfType<DependencyA>();
        instances[1].Should().BeOfType<DependencyB>();
        instances[2].Should().BeOfType<DependencyC>();
    }
    
    private interface IArrayElementA { }
    private interface IArrayElementB { }
    private interface IArrayElementC { }
    private class ArrayElement : IArrayElementA, IArrayElementB, IArrayElementC { }
    
    [Test]
    public void RegisterArrayToOneContract_WhenResolve_ReturnCorrectInstances()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IArrayElementA[], ArrayElement[]>(new ArrayElement[3]
        {
            new ArrayElement(),
            new ArrayElement(),
            new ArrayElement(),
        });
        
        // Asserts
        var container = containerBuilder.Build();
        var instances = container.Resolve<IArrayElementA[]>();
        instances[0].Should().BeOfType<ArrayElement>();
        instances[1].Should().BeOfType<ArrayElement>();
        instances[2].Should().BeOfType<ArrayElement>();
    }
    
    [Test]
    public void RegisterArrayToTwoContract_WhenResolve_ReturnCorrectInstances()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IArrayElementA[], IArrayElementB[], ArrayElement[]>(new ArrayElement[3]
        {
            new ArrayElement(),
            new ArrayElement(),
            new ArrayElement(),
        });
        
        // Asserts
        var container = containerBuilder.Build();
        var instancesA = container.Resolve<IArrayElementA[]>();
        instancesA[0].Should().BeOfType<ArrayElement>();
        instancesA[1].Should().BeOfType<ArrayElement>();
        instancesA[2].Should().BeOfType<ArrayElement>();
        
        var instancesB = container.Resolve<IArrayElementB[]>();
        instancesB[0].Should().BeOfType<ArrayElement>();
        instancesB[1].Should().BeOfType<ArrayElement>();
        instancesB[2].Should().BeOfType<ArrayElement>();
    }
    
    [Test]
    public void RegisterArrayToThreeContract_WhenResolve_ReturnCorrectInstances()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IArrayElementA[], IArrayElementB[], IArrayElementC[], ArrayElement[]>(new ArrayElement[3]
        {
            new ArrayElement(),
            new ArrayElement(),
            new ArrayElement(),
        });
        
        // Asserts
        var container = containerBuilder.Build();
        var instancesA = container.Resolve<IArrayElementA[]>();
        instancesA[0].Should().BeOfType<ArrayElement>();
        instancesA[1].Should().BeOfType<ArrayElement>();
        instancesA[2].Should().BeOfType<ArrayElement>();
        
        var instancesB = container.Resolve<IArrayElementB[]>();
        instancesB[0].Should().BeOfType<ArrayElement>();
        instancesB[1].Should().BeOfType<ArrayElement>();
        instancesB[2].Should().BeOfType<ArrayElement>();
        
        var instancesC = container.Resolve<IArrayElementC[]>();
        instancesC[0].Should().BeOfType<ArrayElement>();
        instancesC[1].Should().BeOfType<ArrayElement>();
        instancesC[2].Should().BeOfType<ArrayElement>();
    }
}