using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class DisposeTest
{
    private class DisposeCounter : IDisposable
    {
        public int Calls { get; private set; }
        
        public void Dispose()
        {
            Calls++;
        }
    } 
    
    [Test]
    public void TransientMarkedAsDisposable_WhenRegisteredWithBuilder_ThrowProperException()
    {
        // Setup
        var builder = new ContainerBuilder();

        // Asserts
        builder.Invoking(subject => subject.Register<DisposeCounter>().MarkDisposable()).Should()
            .Throw<SparseInjectException>();
    }
    
    [Test]
    public void TransientMarkedAsDisposable_WhenMarkedAsDisposable_ThrowProperException()
    {
        // Setup
        var builder = new ContainerBuilder();

        var registrationOptions = builder.Register<DisposeCounter>();
        
        // Asserts
        registrationOptions.Invoking(subject => subject.MarkDisposable()).Should()
            .Throw<SparseInjectException>();
    }
    
    [Test]
    public void SingletonValue_WhenScopeDisposed_DisposeNotCalled()
    {
        // Setup
        var builder = new ContainerBuilder();

        var disposable = Substitute.For<IDisposable>();

        builder.RegisterValue(disposable);
        
        var container = builder.Build();
        
        container.Dispose();

        // Asserts
        disposable.Received(0);
    }
    
    [Test]
    public void SingletonValueMarkedAsDisposable_WhenScopeDisposed_DisposeIsCalled()
    {
        // Setup
        var builder = new ContainerBuilder();

        var disposable = Substitute.For<IDisposable>();
        
        builder.RegisterValue(disposable).MarkDisposable();
        
        var container = builder.Build();
        
        disposable.Received(0);
        
        container.Dispose();

        // Asserts
        disposable.Received(1);
    }
    
    [Test]
    public void SingletonValueMarkedAsDisposableWithBuilder_WhenScopeDisposed_DisposeIsCalled()
    {
        // Setup
        var builder = new ContainerBuilder();

        var disposable = Substitute.For<IDisposable>();

        var registrationOptions = builder.RegisterValue(disposable);
        
        registrationOptions.MarkDisposable();
        
        var container = builder.Build();
        
        disposable.Received(0);
        
        container.Dispose();

        // Asserts
        disposable.Received(1);
    }

    [Test]
    public void SingletonsMarkedAsDisposable_WhenScopeDisposed_DisposeIsCalled()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IDisposable, DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        builder.Register<DisposeCounter>(Lifetime.Singleton).MarkDisposable();

        var container = builder.Build();
        
        var interfaceSingleton = container.Resolve<IDisposable>() as DisposeCounter;
        var singleton = container.Resolve<DisposeCounter>();

        interfaceSingleton.Calls.Should().Be(0);
        singleton.Calls.Should().Be(0);
        
        container.Dispose();
        
        // Asserts
        interfaceSingleton.Calls.Should().Be(1);
        singleton.Calls.Should().Be(1);
    }
    
    [Test]
    public void SingletonsCollectionMarkedAsDisposable_WhenScopeDisposed_DisposeIsCalled()
    {
        // Setup
        var builder = new ContainerBuilder();
        
        var disposables = new IDisposable[]
        {
            new DisposeCounter(),
            new DisposeCounter(),
            new DisposeCounter(),
            new DisposeCounter(),
        };

        builder.RegisterValue(disposables).MarkDisposable();

        var container = builder.Build();

        var instances = container.Resolve<IDisposable[]>();

        foreach (var instance in instances)
        {
            if (instance is DisposeCounter disposableCounter)
            {
                disposableCounter.Calls.Should().Be(0);
            }
        }
        
        container.Dispose();
        
        foreach (var instance in instances)
        {
            if (instance is DisposeCounter disposableCounter)
            {
                disposableCounter.Calls.Should().Be(1);
            }
        }
    }
    
    [Test]
    public void SingletonsJaggedCollectionMarkedAsDisposable_WhenScopeDisposed_DisposeIsCalled()
    {
        // Setup
        var builder = new ContainerBuilder();
        
        var disposables = new IDisposable[]
        {
            new DisposeCounter(),
            new DisposeCounter(),
            new DisposeCounter(),
            new DisposeCounter(),
        };

        builder.RegisterValue(disposables).MarkDisposable();
        builder.RegisterValue(disposables).MarkDisposable();

        var container = builder.Build();

        var instances = container.Resolve<IDisposable[][]>();

        foreach (var instance in instances)
        {
            foreach (var disposable in instance)
            {
                if (disposable is DisposeCounter disposableCounter)
                {
                    disposableCounter.Calls.Should().Be(0);
                }
            }
        }
        
        container.Dispose();
        
        foreach (var instance in instances)
        {
            foreach (var disposable in instance)
            {
                if (disposable is DisposeCounter disposableCounter)
                {
                    disposableCounter.Calls.Should().Be(2);
                }
            }
        }
    }
}