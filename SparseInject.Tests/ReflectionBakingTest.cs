using System;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;
using SparseInject.ReflectionBaking.Tests.Scope;
using SparseInject.ReflectionBaking.Tests.Singleton;
using SparseInject.ReflectionBaking.Tests.Transient;

// TODO: NEED FIXES
[TestFixture]
public class ReflectionBakingTest
{
    [Test]
    public void Transient_Test()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register(TransientTestInstaller.Install);
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<IDisposable>();

        instance.Should().BeOfType<Dependency>();
        
        var instances = container.Resolve<IDisposable[]>();
        
        instances.Length.Should().Be(2);
    }
    
    [Test]
    public void Singleton_Test()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register(SingletonTestInstaller.Install);
        
        var container = containerBuilder.Build();

        // Asserts
        var instance = container.Resolve<IDisposable>();

        instance.Should().BeOfType<Dependency>();
        
        var instances = container.Resolve<IDisposable[]>();
        
        instances.Length.Should().Be(2);
    }
    
    [Test]
    public void Scope_Test()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register(ScopeTestInstaller.Install);
        
        var container = containerBuilder.Build();

        // Asserts
        container.Resolve<Dependency>().Should().BeOfType<Dependency>();
        container.Resolve<ScopeA>().Should().BeOfType<ScopeA>();
        container.Resolve<IScopeB>().Should().BeOfType<ScopeB>();
        container.Resolve<ScopeC>().Should().BeOfType<ScopeC>();
        container.Resolve<IScopeD>().Should().BeOfType<ScopeD>();
    }
}