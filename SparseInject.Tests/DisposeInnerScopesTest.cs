using System;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class DisposeInnerScopesTest
{
    private class DisposeCounter : IDisposable
    {
        public int Calls { get; private set; }
        
        public void Dispose()
        {
            Calls++;
        }
    }
    
    private class ScopeA : Scope
    {
    }
    
    [Test]
    public void RegisterScope_WhenRootDisposedAndChildCallDispose_ThrowObjectDisposedException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterScope<ScopeA>(configurator => { });

        var container = containerBuilder.Build();

        // Asserts
        var scope = container.Resolve<ScopeA>();

        container.Dispose();
        
        scope.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
    }

    [Test]
    public void RegisterScopeWithDisposable_WhenRootDisposed_ChildScopeAndInstanceIsDisposed()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();

        containerBuilder.RegisterScope<ScopeA>(configurator =>
        {
            configurator.Register<DisposeCounter>(Lifetime.Singleton).MarkDisposable();
        });

        var container = containerBuilder.Build();

        // Asserts
        var scope = container.Resolve<ScopeA>();

        var disposable = scope._container.Resolve<DisposeCounter>();
        
        disposable.Calls.Should().Be(0);
        
        container.Dispose();
        
        scope.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();

        disposable.Calls.Should().Be(1);
    }
    
    [Test]
    public void RegisterScopeWithDisposableValue_WhenRootDisposed_ChildScopeAndInstanceIsDisposed()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        var disposable = new DisposeCounter();
        
        containerBuilder.RegisterScope<ScopeA>(configurator =>
        {
            configurator.RegisterValue(disposable).MarkDisposable();
        });

        var container = containerBuilder.Build();

        // Asserts
        var scope = container.Resolve<ScopeA>();
        
        container.Dispose();
        
        scope.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();

        disposable.Calls.Should().Be(1);
    }

    private class InnerScopeA : Scope { }
    private class InnerScopeB : Scope { }
    private class InnerScopeC : Scope { }
    private class InnerScopeD : Scope { }
    
    [Test]
    public void InnerScopesRegisteredInsideParents_WhenRootDisposed_ChildDisposed()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        var disposable = new DisposeCounter();
        var disposableA = new DisposeCounter();
        var disposableB = new DisposeCounter();
        var disposableC = new DisposeCounter();
        var disposableD = new DisposeCounter();
        
        containerBuilder.RegisterValue(disposable).MarkDisposable();
        containerBuilder.RegisterScope<InnerScopeA>(configurationA =>
        {
            configurationA.RegisterValue(disposableA).MarkDisposable();
            configurationA.RegisterScope<InnerScopeB>(configurationB =>
            {
                configurationB.RegisterValue(disposableB).MarkDisposable();
                configurationB.RegisterScope<InnerScopeC>(configurationC =>
                {
                    configurationC.RegisterValue(disposableC).MarkDisposable();
                    configurationC.RegisterScope<InnerScopeD>(configurationD =>
                    {
                        configurationD.RegisterValue(disposableD).MarkDisposable();
                    });
                });
            });
        });

        var container = containerBuilder.Build();
        var scopeA = container.Resolve<InnerScopeA>();
        var scopeB = scopeA._container.Resolve<InnerScopeB>();
        var scopeC = scopeB._container.Resolve<InnerScopeC>();
        var scopeD = scopeC._container.Resolve<InnerScopeD>();
        
        // Asserts
        disposable.Calls.Should().Be(0);
        disposableA.Calls.Should().Be(0);
        disposableB.Calls.Should().Be(0);
        disposableC.Calls.Should().Be(0);
        disposableD.Calls.Should().Be(0);
        
        container.Dispose();
        
        disposable.Calls.Should().Be(1);
        disposableA.Calls.Should().Be(1);
        disposableB.Calls.Should().Be(1);
        disposableC.Calls.Should().Be(1);
        disposableD.Calls.Should().Be(1);
        
        container.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        scopeA.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        scopeB.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        scopeC.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        scopeD.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        
        disposable.Calls.Should().Be(1);
        disposableA.Calls.Should().Be(1);
        disposableB.Calls.Should().Be(1);
        disposableC.Calls.Should().Be(1);
        disposableD.Calls.Should().Be(1);
    }
    
    [Test]
    public void InnerScopesRegisteredInsideParents_WhenMiddleScopeDisposed_ChildDisposedAndParentsNotDisposed()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        var disposable = new DisposeCounter();
        var disposableA = new DisposeCounter();
        var disposableB = new DisposeCounter();
        var disposableC = new DisposeCounter();
        var disposableD = new DisposeCounter();
        
        containerBuilder.RegisterValue(disposable).MarkDisposable();
        containerBuilder.RegisterScope<InnerScopeA>(configurationA =>
        {
            configurationA.RegisterValue(disposableA).MarkDisposable();
            configurationA.RegisterScope<InnerScopeB>(configurationB =>
            {
                configurationB.RegisterValue(disposableB).MarkDisposable();
                configurationB.RegisterScope<InnerScopeC>(configurationC =>
                {
                    configurationC.RegisterValue(disposableC).MarkDisposable();
                    configurationC.RegisterScope<InnerScopeD>(configurationD =>
                    {
                        configurationD.RegisterValue(disposableD).MarkDisposable();
                    });
                });
            });
        });

        var container = containerBuilder.Build();
        var scopeA = container.Resolve<InnerScopeA>();
        var scopeB = scopeA._container.Resolve<InnerScopeB>();
        var scopeC = scopeB._container.Resolve<InnerScopeC>();
        var scopeD = scopeC._container.Resolve<InnerScopeD>();
        
        // Asserts
        disposable.Calls.Should().Be(0);
        disposableA.Calls.Should().Be(0);
        disposableB.Calls.Should().Be(0);
        disposableC.Calls.Should().Be(0);
        disposableD.Calls.Should().Be(0);
        
        scopeC.Dispose();
        
        disposable.Calls.Should().Be(0);
        disposableA.Calls.Should().Be(0);
        disposableB.Calls.Should().Be(0);
        disposableC.Calls.Should().Be(1);
        disposableD.Calls.Should().Be(1);
        
        scopeD.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        scopeC.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        scopeB.Invoking(subject => subject.Dispose()).Should().NotThrow();
        scopeA.Invoking(subject => subject.Dispose()).Should().NotThrow();
        container.Invoking(subject => subject.Dispose()).Should().NotThrow();
        
        disposable.Calls.Should().Be(1);
        disposableA.Calls.Should().Be(1);
        disposableB.Calls.Should().Be(1);
        disposableC.Calls.Should().Be(1);
        disposableD.Calls.Should().Be(1);
    }
    
    [Test]
    public void InnerScopesRegisteredInMain_WhenRootDisposed_ChildDisposed()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        var disposable = new DisposeCounter();
        var disposableA = new DisposeCounter();
        var disposableB = new DisposeCounter();
        var disposableC = new DisposeCounter();
        var disposableD = new DisposeCounter();
        
        containerBuilder.RegisterValue(disposable).MarkDisposable();
        containerBuilder.RegisterScope<InnerScopeA>(configurationA =>
        {
            configurationA.RegisterValue(disposableA).MarkDisposable();
        });
        containerBuilder.RegisterScope<InnerScopeB>(configurationB =>
        {
            configurationB.RegisterValue(disposableB).MarkDisposable();
        });
        containerBuilder.RegisterScope<InnerScopeC>(configurationC =>
        {
            configurationC.RegisterValue(disposableC).MarkDisposable();
        });
        containerBuilder.RegisterScope<InnerScopeD>(configurationD =>
        {
            configurationD.RegisterValue(disposableD).MarkDisposable();
        });

        var container = containerBuilder.Build();
        var scopeA = container.Resolve<InnerScopeA>();
        var scopeB = container.Resolve<InnerScopeB>();
        var scopeC = container.Resolve<InnerScopeC>();
        var scopeD = container.Resolve<InnerScopeD>();
        
        // Asserts
        disposable.Calls.Should().Be(0);
        disposableA.Calls.Should().Be(0);
        disposableB.Calls.Should().Be(0);
        disposableC.Calls.Should().Be(0);
        disposableD.Calls.Should().Be(0);
        
        container.Dispose();
        
        disposable.Calls.Should().Be(1);
        disposableA.Calls.Should().Be(1);
        disposableB.Calls.Should().Be(1);
        disposableC.Calls.Should().Be(1);
        disposableD.Calls.Should().Be(1);
        
        container.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        scopeA.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        scopeB.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        scopeC.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        scopeD.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        
        disposable.Calls.Should().Be(1);
        disposableA.Calls.Should().Be(1);
        disposableB.Calls.Should().Be(1);
        disposableC.Calls.Should().Be(1);
        disposableD.Calls.Should().Be(1);
    }
    
    [Test]
    public void InnerScopesRegisteredInMain_WhenChildNotCreatedAndRootDisposed_ChildInstancesNotDisposed()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        var disposable = new DisposeCounter();
        var disposableA = new DisposeCounter();
        var disposableB = new DisposeCounter();
        var disposableC = new DisposeCounter();
        var disposableD = new DisposeCounter();
        
        containerBuilder.RegisterValue(disposable).MarkDisposable();
        containerBuilder.RegisterScope<InnerScopeA>(configurationA =>
        {
            configurationA.RegisterValue(disposableA).MarkDisposable();
        });
        containerBuilder.RegisterScope<InnerScopeB>(configurationB =>
        {
            configurationB.RegisterValue(disposableB).MarkDisposable();
        });
        containerBuilder.RegisterScope<InnerScopeC>(configurationC =>
        {
            configurationC.RegisterValue(disposableC).MarkDisposable();
        });
        containerBuilder.RegisterScope<InnerScopeD>(configurationD =>
        {
            configurationD.RegisterValue(disposableD).MarkDisposable();
        });

        var container = containerBuilder.Build();

        // Asserts
        disposable.Calls.Should().Be(0);
        disposableA.Calls.Should().Be(0);
        disposableB.Calls.Should().Be(0);
        disposableC.Calls.Should().Be(0);
        disposableD.Calls.Should().Be(0);
        
        container.Dispose();
        
        disposable.Calls.Should().Be(1);
        disposableA.Calls.Should().Be(0);
        disposableB.Calls.Should().Be(0);
        disposableC.Calls.Should().Be(0);
        disposableD.Calls.Should().Be(0);
        
        container.Invoking(subject => subject.Dispose()).Should().Throw<ObjectDisposedException>();
        
        disposable.Calls.Should().Be(1);
        disposableA.Calls.Should().Be(0);
        disposableB.Calls.Should().Be(0);
        disposableC.Calls.Should().Be(0);
        disposableD.Calls.Should().Be(0);
    }
}