using System;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

public class ScopeDisposeTest
{
    [Test]
    public void Container_WhenDisposed_ThrowExceptionOnResolve()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        var container = containerBuilder.Build();
        
        container.Dispose();

        // Asserts
        container
            .Invoking(subject => subject.Resolve<IDisposable[]>())
            .Should()
            .Throw<Exception>();
    }
    
    [Test]
    public void Container_WhenDisposeTwice_ThrowException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        var container = containerBuilder.Build();

        container.Dispose();

        // Asserts
        container
            .Invoking(subject => subject.Dispose())
            .Should()
            .Throw<Exception>();
    }
    
    private class ScopeA : Scope { }
    
    [Test]
    public void Scope_WhenDisposed_ThrowExceptionOnResolve()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterScope<ScopeA>(_ => { });
        
        var container = containerBuilder.Build();
        
        var scopeA = container.Resolve<ScopeA>();
        
        scopeA.Dispose();

        // Asserts
        scopeA
            .Invoking(subject => subject._container.Resolve<IDisposable[]>())
            .Should()
            .Throw<Exception>();
    }
    
    [Test]
    public void Scope_WhenDisposeTwice_ThrowException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterScope<ScopeA>(_ => { });
        
        var container = containerBuilder.Build();
        
        var scopeA = container.Resolve<ScopeA>();
        
        scopeA.Dispose();

        // Asserts
        scopeA._container
            .Invoking(subject => subject.Dispose())
            .Should()
            .Throw<Exception>();
    }

    private class MainScopeDependency : IDisposable { public void Dispose() { } }
    
    [Test]
    public void MainContainer_WhenInnerScopeDisposeD_CanResolve()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<MainScopeDependency>();
        containerBuilder.RegisterScope<ScopeA>(_ => { });
        
        var container = containerBuilder.Build();
        
        var scopeA = container.Resolve<ScopeA>();
        
        scopeA.Dispose();

        // Asserts
        container.Resolve<MainScopeDependency>().Should().BeOfType<MainScopeDependency>();
    }
}