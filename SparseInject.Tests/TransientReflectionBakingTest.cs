using FluentAssertions;
using NUnit.Framework;
using SparseInject;
using SparseInject.ReflectionBaking.Tests.Transient;

[TestFixture]
public class TransientReflectionBakingTest
{
    [Test]
    public void TransientConcreteTypes_WhenAccessingInstanceFactory_ReturnInstanceFactory()
    {
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyA), out var factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyB), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyC), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyD), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
    }
    
    [Test]
    public void TransientContractTypes_WhenAccessingInstanceFactory_ReturnNull()
    {
        // Asserts B
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyB), out var factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts C
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyC0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyC1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts D
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyD0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyD1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyD2), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
    }

    [Test]
    public void TransientRegisterApi_WhenResolving_WorkProperly()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register(TransientTestInstaller.Install);
        
        var container = containerBuilder.Build();
        
        // Asserts A
        var a00 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyA>();
        var a01 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyA>();
        a00.Should().BeOfType<SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyA>();
        a00.Should().NotBe(a01);
        
        // Asserts B
        var b00 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyB>();
        var b01 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyB>();
        b00.Should().BeOfType<SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyB>();
        b00.Should().NotBe(b01);
        
        // Asserts C
        var c00 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyC0>();
        var c01 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyC0>();
        c00.Should().BeOfType<SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyC>();
        c00.Should().NotBe(c01);
        
        var c10 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyC1>();
        var c11 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyC1>();
        c10.Should().BeOfType<SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyC>();
        c10.Should().NotBe(c11);

        c00.Should().NotBe(c10);
        
        // Asserts D
        var d00 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyD0>();
        var d01 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyD0>();
        d00.Should().BeOfType<SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyD>();
        d00.Should().NotBe(d01);
        
        var d10 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyD1>();
        var d11 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyD1>();
        d10.Should().BeOfType<SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyD>();
        d10.Should().NotBe(d11);
        
        var d20 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyD2>();
        var d21 = container.Resolve<SparseInject.ReflectionBaking.Tests.Transient.ITransientDependencyD2>();
        d10.Should().BeOfType<SparseInject.ReflectionBaking.Tests.Transient.TransientDependencyD>();
        d20.Should().NotBe(d21);
        
        d00.Should().NotBe(d10);
        d10.Should().NotBe(d20);
    }
}