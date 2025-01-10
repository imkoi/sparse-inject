using FluentAssertions;
using NUnit.Framework;
using SparseInject;
using SparseInject.ReflectionBaking.Tests.NestedClass;

public class NestedClassReflectionBakingTest
{
    [Test]
    public void ConcreteTypes_WhenAccessingInstanceFactory_ReturnInstanceFactory()
    {
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyA), out var factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyB), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyC), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyD), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
    }
    
    [Test]
    public void ContractTypes_WhenAccessingInstanceFactory_ReturnNull()
    {
        // Asserts B
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyB), out var factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts C
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyC0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyC1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts D
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyD0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyD1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyD2), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
    }

    [Test]
    public void RegisterApi_WhenResolving_WorkProperly()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register(NestedClassTestInstaller.Install);
        
        var container = containerBuilder.Build();
        
        // Asserts A
        var a00 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyA>();
        var a01 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyA>();
        a00.Should().BeOfType<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyA>();
        a00.Should().NotBe(a01);
        
        // Asserts B
        var b00 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyB>();
        var b01 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyB>();
        b00.Should().BeOfType<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyB>();
        b00.Should().NotBe(b01);
        
        // Asserts C
        var c00 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyC0>();
        var c01 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyC0>();
        c00.Should().BeOfType<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyC>();
        c00.Should().NotBe(c01);
        
        var c10 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyC1>();
        var c11 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyC1>();
        c10.Should().BeOfType<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyC>();
        c10.Should().NotBe(c11);

        c00.Should().NotBe(c10);
        
        // Asserts D
        var d00 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyD0>();
        var d01 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyD0>();
        d00.Should().BeOfType<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyD>();
        d00.Should().NotBe(d01);
        
        var d10 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyD1>();
        var d11 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyD1>();
        d10.Should().BeOfType<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyD>();
        d10.Should().NotBe(d11);
        
        var d20 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyD2>();
        var d21 = container.Resolve<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.ITransientDependencyD2>();
        d10.Should().BeOfType<SparseInject.ReflectionBaking.Tests.NestedClass.NestedClassTestInstaller.TransientDependencyD>();
        d20.Should().NotBe(d21);
        
        d00.Should().NotBe(d10);
        d10.Should().NotBe(d20);
    }
}