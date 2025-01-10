using System;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SparseInject;

public class FactoryCollectionsTest
{
    private class Player : IPlayer
    {
        public int MaxHealth { get; set; }
        public IAudioService AudioService { get; set; }
    }

    private interface IPlayer
    {
        int MaxHealth { get; }
        IAudioService AudioService { get; }
    }

    public interface IAudioService
    {
    }

    [Test]
    public void RegisteredFactory_WhenResolvedAndInvoked_ReturnCorrectValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterFactory(() => new Player());

        var container = builder.Build();

        // Asserts
        var value = container.Resolve<Func<Player>[]>();

        value.Length.Should().Be(1);
        value.First().Invoke().Should().BeOfType<Player>();
    }

    [Test]
    public void RegisteredFactory_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterFactory(() => new Player());
        builder.RegisterFactory(() => new Player());

        var container = builder.Build();
        var factories = container.Resolve<Func<Player>[]>();
        
        // Asserts
        factories.Length.Should().Be(2);
        
        var firstValue = factories[0].Invoke();
        var secondValue = factories[1].Invoke();

        firstValue.Should().NotBe(secondValue);
    }

    [Test]
    public void RegisteredFactoryToInterface_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterFactory<IPlayer>(() => new Player());
        builder.RegisterFactory<IPlayer>(() => new Player());

        var container = builder.Build();
        var factories = container.Resolve<Func<IPlayer>[]>();

        // Asserts
        factories.Length.Should().Be(2);
        
        var firstValue = factories[0].Invoke();
        var secondValue = factories[1].Invoke();

        firstValue.Should().NotBe(secondValue);
    }

    [Test]
    public void RegisteredFactory_WhenResolvedMultipleTimes_ReturnSameFactory()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterFactory(() => new Player());

        var container = builder.Build();

        // Asserts
        var firstFactories = container.Resolve<Func<Player>[]>();
        var secondFactories = container.Resolve<Func<Player>[]>();

        firstFactories[0].Should().BeSameAs(secondFactories[0]);
    }

    [Test]
    public void RegisteredFactoryWithParameter_WhenResolvedAndInvoked_ReturnCorrectValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterFactory<int, Player>(parameter => new Player
        {
            MaxHealth = parameter
        });
        builder.RegisterFactory<int, Player>(parameter => new Player
        {
            MaxHealth = parameter
        });

        var container = builder.Build();

        // Asserts
        var value = container.Resolve<Func<int, Player>>().Invoke(100);

        value.Should().BeOfType<Player>();
        value.MaxHealth.Should().Be(100);
    }

    [Test]
    public void RegisteredFactoryWithParameter_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterFactory<int, Player>(parameter => new Player { MaxHealth = parameter });
        builder.RegisterFactory<int, Player>(parameter => new Player { MaxHealth = parameter });

        var container = builder.Build();
        var factories = container.Resolve<Func<int, Player>[]>();

        // Asserts
        var firstValue = factories[0].Invoke(100);
        var secondValue = factories[1].Invoke(25);

        firstValue.Should().NotBe(secondValue);

        firstValue.MaxHealth.Should().Be(100);
        secondValue.MaxHealth.Should().Be(25);
    }

    [Test]
    public void RegisteredFactoryToInterfaceWithParameter_WhenResolvedAndInvokedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterFactory<int, IPlayer, Player>(parameter => new Player { MaxHealth = parameter });
        builder.RegisterFactory<int, IPlayer, Player>(parameter => new Player { MaxHealth = parameter });

        var container = builder.Build();
        var factories = container.Resolve<Func<int, IPlayer>[]>();

        // Asserts
        var firstValue = factories[0].Invoke(100);
        var secondValue = factories[1].Invoke(25);

        firstValue.Should().NotBe(secondValue);

        firstValue.MaxHealth.Should().Be(100);
        secondValue.MaxHealth.Should().Be(25);
    }
}