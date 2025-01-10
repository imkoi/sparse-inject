using FluentAssertions;
using NUnit.Framework;
using SparseInject;
using SparseInject.ReflectionBaking.Tests.Scope;

public class ScopeReflectionBakingTest
{
    [Test]
    public void ScopeConcreteAndRegistrationsTypes_WhenAccessingInstanceFactory_ReturnInstanceFactory()
    {
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ScopeA), out var factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ScopeB), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ScopeC), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ScopeD), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(Dependency), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(DependencyA), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(DependencyB), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(DependencyC), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(DependencyD), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
    }
    
    [Test]
    public void ScopeContractTypes_WhenAccessingInstanceFactory_ReturnNull()
    {
        // Asserts B
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IScopeB), out var factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts C
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IScopeD), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
    }
    
    [Test]
    public void ScopeRegisterApi_WhenResolving_WorkProperly()
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