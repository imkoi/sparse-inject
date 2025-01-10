using FluentAssertions;
using NUnit.Framework;
using SparseInject;
using SparseInject.ReflectionBaking.Tests.GlobalNamespace;

public class GlobalNamespaceReflectionBakingTest
{
    [Test]
    public void ConcreteTypes_WhenAccessingInstanceFactory_ReturnInstanceFactory()
    {
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(TransientDependencyA), out var factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(TransientDependencyB), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(TransientDependencyC), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(TransientDependencyD), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
    }
    
    [Test]
    public void ContractTypes_WhenAccessingInstanceFactory_ReturnNull()
    {
        // Asserts B
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ITransientDependencyB), out var factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts C
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ITransientDependencyC0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ITransientDependencyC1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts D
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ITransientDependencyD0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ITransientDependencyD1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(ITransientDependencyD2), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
    }

    [Test]
    public void RegisterApi_WhenResolving_WorkProperly()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register(GlobalNamespaceTestInstaller.Install);
        
        var container = containerBuilder.Build();
        
        // Asserts A
        var a00 = container.Resolve<TransientDependencyA>();
        var a01 = container.Resolve<TransientDependencyA>();
        a00.Should().BeOfType<TransientDependencyA>();
        a00.Should().NotBe(a01);
        
        // Asserts B
        var b00 = container.Resolve<ITransientDependencyB>();
        var b01 = container.Resolve<ITransientDependencyB>();
        b00.Should().BeOfType<TransientDependencyB>();
        b00.Should().NotBe(b01);
        
        // Asserts C
        var c00 = container.Resolve<ITransientDependencyC0>();
        var c01 = container.Resolve<ITransientDependencyC0>();
        c00.Should().BeOfType<TransientDependencyC>();
        c00.Should().NotBe(c01);
        
        var c10 = container.Resolve<ITransientDependencyC1>();
        var c11 = container.Resolve<ITransientDependencyC1>();
        c10.Should().BeOfType<TransientDependencyC>();
        c10.Should().NotBe(c11);

        c00.Should().NotBe(c10);
        
        // Asserts D
        var d00 = container.Resolve<ITransientDependencyD0>();
        var d01 = container.Resolve<ITransientDependencyD0>();
        d00.Should().BeOfType<TransientDependencyD>();
        d00.Should().NotBe(d01);
        
        var d10 = container.Resolve<ITransientDependencyD1>();
        var d11 = container.Resolve<ITransientDependencyD1>();
        d10.Should().BeOfType<TransientDependencyD>();
        d10.Should().NotBe(d11);
        
        var d20 = container.Resolve<ITransientDependencyD2>();
        var d21 = container.Resolve<ITransientDependencyD2>();
        d10.Should().BeOfType<TransientDependencyD>();
        d20.Should().NotBe(d21);
        
        d00.Should().NotBe(d10);
        d10.Should().NotBe(d20);
    }
}