using FluentAssertions;
using NUnit.Framework;
using SparseInject;
using SparseInject.ReflectionBaking.Tests.Singleton;

[TestFixture]
public class SingletonReflectionBakingTest
{
    [Test]
    public void SingletonConcreteTypes_WhenAccessingInstanceFactory_ReturnInstanceFactory()
    {
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SingletonDependencyA), out var factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SingletonDependencyB), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SingletonDependencyC), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SingletonDependencyD), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
    }
    
    [Test]
    public void SingletonContractTypes_WhenAccessingInstanceFactory_ReturnNull()
    {
        // Asserts B
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ISingletonDependencyB), out var factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts C
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ISingletonDependencyC0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ISingletonDependencyC1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts D
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ISingletonDependencyD0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ISingletonDependencyD1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ISingletonDependencyD2), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
    }
    
    [Test]
    public void SingletonRegisterApi_WhenResolving_WorkProperly()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register(SingletonTestInstaller.Install);
        
        var container = containerBuilder.Build();

        // Asserts A
        var a00 = container.Resolve<SingletonDependencyA>();
        var a01 = container.Resolve<SingletonDependencyA>();
        a00.Should().BeOfType<SingletonDependencyA>();
        a00.Should().Be(a01);
        
        // Asserts B
        var b00 = container.Resolve<ISingletonDependencyB>();
        var b01 = container.Resolve<ISingletonDependencyB>();
        b00.Should().BeOfType<SingletonDependencyB>();
        b00.Should().Be(b01);
        
        // Asserts C
        var c00 = container.Resolve<ISingletonDependencyC0>();
        var c01 = container.Resolve<ISingletonDependencyC0>();
        c00.Should().BeOfType<SingletonDependencyC>();
        c00.Should().Be(c01);
        
        var c10 = container.Resolve<ISingletonDependencyC1>();
        var c11 = container.Resolve<ISingletonDependencyC1>();
        c10.Should().BeOfType<SingletonDependencyC>();
        c10.Should().Be(c11);

        c00.Should().Be(c10);
        
        // Asserts D
        var d00 = container.Resolve<ISingletonDependencyD0>();
        var d01 = container.Resolve<ISingletonDependencyD0>();
        d00.Should().BeOfType<SingletonDependencyD>();
        d00.Should().Be(d01);
        
        var d10 = container.Resolve<ISingletonDependencyD1>();
        var d11 = container.Resolve<ISingletonDependencyD1>();
        d10.Should().BeOfType<SingletonDependencyD>();
        d10.Should().Be(d11);
        
        var d20 = container.Resolve<ISingletonDependencyD2>();
        var d21 = container.Resolve<ISingletonDependencyD2>();
        d10.Should().BeOfType<SingletonDependencyD>();
        d20.Should().Be(d21);
        
        d00.Should().Be(d10);
        d10.Should().Be(d20);
    }
}