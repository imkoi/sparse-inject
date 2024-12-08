using System;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;
using SparseInject.ReflectionBaking.Tests.PartialBaking;

[TestFixture]
public class PartialReflectionBakingTest
{
    [Test]
    public void BakedConcreteTypes_WhenAccessingInstanceFactory_ReturnInstanceFactory()
    {
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(BakedDependencyA), out var factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(BakedDependencyB), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(BakedDependencyC), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(BakedDependencyD), out factory, out _)
            .Should().BeTrue();
        factory.Should().NotBeNull();
    }
    
    [Test]
    public void UnbakedConcreteTypes_WhenAccessingInstanceFactory_ReturnNull()
    {
        // Asserts B
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(UnbakedDependencyA), out var factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts C
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(UnbakedDependencyB), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(UnbakedDependencyC), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts D
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(UnbakedDependencyD), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
    }
    
    [Test]
    public void ContractTypes_WhenAccessingInstanceFactory_ReturnNull()
    {
        // Asserts B
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IDependencyB), out var factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts C
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IDependencyC0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IDependencyC1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        // Asserts D
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IDependencyD0), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();

        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IDependencyD1), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
        
        ReflectionBakingProviderCache.TryGetInstanceFactory(typeof(IDependencyD2), out factory, out _)
            .Should().BeFalse();
        factory.Should().BeNull();
    }

    [Test]
    public void RegisterApi_WhenResolving_WorkProperly()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register(PartialBakingTestInstaller.Install);
        
        var container = containerBuilder.Build();
        
        // Asserts A
        var a00 = container.Resolve<BakedDependencyA>();
        var a01 = container.Resolve<BakedDependencyA>();
        a00.Should().BeOfType<BakedDependencyA>();
        a00.Should().NotBe(a01);
        
        // Asserts A unbaked
        var ua00 = container.Resolve<UnbakedDependencyA>();
        var ua01 = container.Resolve<UnbakedDependencyA>();
        ua00.Should().BeOfType<UnbakedDependencyA>();
        ua00.Should().Be(ua01);
        
        // Asserts B
        var b00 = container.Resolve<IDependencyB>();
        var b01 = container.Resolve<IDependencyB>();
        b00.Should().BeOfType<UnbakedDependencyB>();
        b00.Should().Be(b01); // because its re-registered as singleton
        
        // Asserts C
        var c00 = container.Resolve<IDependencyC0>();
        var c01 = container.Resolve<IDependencyC0>();
        c00.Should().BeOfType<UnbakedDependencyC>();
        c00.Should().Be(c01); // because its re-registered as singleton
        
        var c10 = container.Resolve<IDependencyC1>();
        var c11 = container.Resolve<IDependencyC1>();
        c10.Should().BeOfType<UnbakedDependencyC>();
        c10.Should().Be(c11); // because its re-registered as singleton
        
        c00.Should().Be(c10);
        
        // Asserts D
        var d00 = container.Resolve<IDependencyD0>();
        var d01 = container.Resolve<IDependencyD0>();
        d00.Should().BeOfType<UnbakedDependencyD>();
        d00.Should().Be(d01); // because its re-registered as singleton
        
        var d10 = container.Resolve<IDependencyD1>();
        var d11 = container.Resolve<IDependencyD1>();
        d10.Should().BeOfType<UnbakedDependencyD>();
        d10.Should().Be(d11); // because its re-registered as singleton
        
        var d20 = container.Resolve<IDependencyD2>();
        var d21 = container.Resolve<IDependencyD2>();
        d10.Should().BeOfType<UnbakedDependencyD>();
        d20.Should().Be(d21); // because its re-registered as singleton
        
        d00.Should().Be(d10);
        d10.Should().Be(d20);
        
        // Asserts factories
        var factoryConcrete = container.Resolve<Func<UnbakedDependencyB>>();
        factoryConcrete.Should().BeOfType<Func<UnbakedDependencyB>>();
        factoryConcrete.Invoke().Should().BeOfType<UnbakedDependencyB>();

        var factoryContract = container.Resolve<Func<IDependencyB>>();
        factoryContract.Should().BeOfType<Func<IDependencyB>>();
        factoryContract.Invoke().Should().BeOfType<UnbakedDependencyB>();
    }
}