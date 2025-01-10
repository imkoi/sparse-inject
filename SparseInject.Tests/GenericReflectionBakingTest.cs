using FluentAssertions;
using NUnit.Framework;
using SparseInject;
using SparseInject.ReflectionBaking.Tests.Generic;

public class GenericReflectionBakingTest
{
    [Test]
    public void ConcreteTypes_WhenAccessingInstanceFactory_ReturnInstanceFactory()
    {
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(GenericDependencyA<string>), out var factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(GenericDependencyB<string>), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(GenericDependencyC<string>), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(GenericDependencyD<string>), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
    }
    
    [Test]
    public void ContractTypes_WhenAccessingInstanceFactory_ReturnNull()
    {
        // Asserts B
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IGenericDependencyB), out var factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts C
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IGenericDependencyC0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IGenericDependencyC1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts D
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IGenericDependencyD0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IGenericDependencyD1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IGenericDependencyD2), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
    }

    [Test]
    public void RegisterApi_WhenResolving_WorkProperly()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register(GenericTestInstaller.Install);
        
        var container = containerBuilder.Build();
        
        // Asserts A
        var a00 = container.Resolve<GenericDependencyA<string>>();
        var a01 = container.Resolve<GenericDependencyA<string>>();
        a00.Should().BeOfType<GenericDependencyA<string>>();
        a00.Should().NotBe(a01);
        
        // Asserts B
        var b00 = container.Resolve<IGenericDependencyB>();
        var b01 = container.Resolve<IGenericDependencyB>();
        b00.Should().BeOfType<GenericDependencyB<string>>();
        b00.Should().NotBe(b01);
        
        // Asserts C
        var c00 = container.Resolve<IGenericDependencyC0>();
        var c01 = container.Resolve<IGenericDependencyC0>();
        c00.Should().BeOfType<GenericDependencyC<string>>();
        c00.Should().NotBe(c01);
        
        var c10 = container.Resolve<IGenericDependencyC1>();
        var c11 = container.Resolve<IGenericDependencyC1>();
        c10.Should().BeOfType<GenericDependencyC<string>>();
        c10.Should().NotBe(c11);

        c00.Should().NotBe(c10);
        
        // Asserts D
        var d00 = container.Resolve<IGenericDependencyD0>();
        var d01 = container.Resolve<IGenericDependencyD0>();
        d00.Should().BeOfType<GenericDependencyD<string>>();
        d00.Should().NotBe(d01);
        
        var d10 = container.Resolve<IGenericDependencyD1>();
        var d11 = container.Resolve<IGenericDependencyD1>();
        d10.Should().BeOfType<GenericDependencyD<string>>();
        d10.Should().NotBe(d11);
        
        var d20 = container.Resolve<IGenericDependencyD2>();
        var d21 = container.Resolve<IGenericDependencyD2>();
        d10.Should().BeOfType<GenericDependencyD<string>>();
        d20.Should().NotBe(d21);
        
        d00.Should().NotBe(d10);
        d10.Should().NotBe(d20);
    }
}