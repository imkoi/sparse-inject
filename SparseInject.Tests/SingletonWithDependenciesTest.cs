using FluentAssertions;
using NUnit.Framework;
using SparseInject;

public class SingletonWithDependenciesTest
{
    private class PlayerWithDependencies : IPlayerWithDependencies, IPlayerTwo, IPlayerThree
    {
        public IPlayerSingletonDependency SingletonDependency { get; }
        public IPlayerTransientDependency TransientDependency { get; }

        public PlayerWithDependencies(
            IPlayerSingletonDependency singletonDependency,
            IPlayerTransientDependency transientDependency)
        {
            SingletonDependency = singletonDependency;
            TransientDependency = transientDependency;
        }
    }

    private class PlayerTransientDependency : IPlayerTransientDependency { }
    private class PlayerSingletonDependency : IPlayerSingletonDependency { }

    private interface IPlayerWithDependencies
    {
        IPlayerSingletonDependency SingletonDependency { get; }
        IPlayerTransientDependency TransientDependency { get; }
    }

    private interface IPlayerTwo : IPlayerWithDependencies
    {
        IPlayerSingletonDependency SingletonDependency { get; }
        IPlayerTransientDependency TransientDependency { get; }
    }

    private interface IPlayerThree : IPlayerWithDependencies
    {
        IPlayerSingletonDependency SingletonDependency { get; }
        IPlayerTransientDependency TransientDependency { get; }
    }

    private interface IPlayerTransientDependency { }
    private interface IPlayerSingletonDependency { }

    [Test]
    public void Registered_WhenResolved_ReturnCorrectValueWithDependencies()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayerSingletonDependency, PlayerSingletonDependency>(Lifetime.Singleton);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        var value = container.Resolve<PlayerWithDependencies>();

        value.Should().BeOfType<PlayerWithDependencies>();
        value.TransientDependency.Should().BeOfType<PlayerTransientDependency>();
        value.SingletonDependency.Should().BeOfType<PlayerSingletonDependency>();
    }

    [Test]
    public void RegisteredToInterface_WhenResolved_ReturnCorrectValueWithDependencies()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayerSingletonDependency, PlayerSingletonDependency>(Lifetime.Singleton);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        var value = container.Resolve<IPlayerWithDependencies>();

        value.Should().BeOfType<PlayerWithDependencies>();
    }

    [Test]
    public void RegisteredWithoutDependencies_WhenContainerBuilding_ThrowProperException()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<PlayerWithDependencies>(Lifetime.Singleton);

        // Asserts
        builder.Invoking(subject => subject.Build())
            .Should()
            .Throw<SparseInjectException>();
    }

    [Test]
    public void RegisteredToInterface_WhenResolvedNotInterface_ThrowProperException()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayerSingletonDependency, PlayerSingletonDependency>(Lifetime.Singleton);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        container.Invoking(subject => subject.Resolve<PlayerWithDependencies>())
            .Should()
            .Throw<SparseInjectException>();
    }

    [Test]
    public void Registered_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayerSingletonDependency, PlayerSingletonDependency>(Lifetime.Singleton);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<PlayerWithDependencies>();
        var secondValue = container.Resolve<PlayerWithDependencies>();

        firstValue.Should().Be(secondValue);
        firstValue.SingletonDependency.Should().Be(secondValue.SingletonDependency);
        firstValue.TransientDependency.Should().Be(secondValue.TransientDependency);
    }

    [Test]
    public void RegisteredToInterface_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayerSingletonDependency, PlayerSingletonDependency>(Lifetime.Singleton);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayerWithDependencies>();
        var secondValue = container.Resolve<IPlayerWithDependencies>();
        var thirdValue = container.Resolve<IPlayerWithDependencies>();

        firstValue.Should().Be(secondValue);
        firstValue.Should().Be(thirdValue);

        firstValue.SingletonDependency.Should().Be(secondValue.SingletonDependency);
        firstValue.SingletonDependency.Should().Be(thirdValue.SingletonDependency);

        firstValue.TransientDependency.Should().Be(secondValue.TransientDependency);
        firstValue.TransientDependency.Should().Be(thirdValue.TransientDependency);
    }

    [Test]
    public void RegisteredToTwoInterfaces_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayerSingletonDependency, PlayerSingletonDependency>(Lifetime.Singleton);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, IPlayerTwo, PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayerWithDependencies>();
        var secondValue = container.Resolve<IPlayerTwo>();

        firstValue.Should().Be(secondValue);

        firstValue.SingletonDependency.Should().Be(secondValue.SingletonDependency);

        firstValue.TransientDependency.Should().Be(secondValue.TransientDependency);
    }

    [Test]
    public void RegisteredToThreeInterfaces_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayerSingletonDependency, PlayerSingletonDependency>(Lifetime.Singleton);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, IPlayerTwo, IPlayerThree, PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayerWithDependencies>();
        var secondValue = container.Resolve<IPlayerTwo>();
        var thirdValue = container.Resolve<IPlayerThree>();

        firstValue.Should().Be(secondValue);
        firstValue.Should().Be(thirdValue);

        firstValue.Should().BeOfType<PlayerWithDependencies>();
        secondValue.Should().BeOfType<PlayerWithDependencies>();
        thirdValue.Should().BeOfType<PlayerWithDependencies>();

        firstValue.SingletonDependency.Should().Be(secondValue.SingletonDependency);
        firstValue.SingletonDependency.Should().Be(thirdValue.SingletonDependency);

        firstValue.TransientDependency.Should().Be(secondValue.TransientDependency);
        firstValue.TransientDependency.Should().Be(thirdValue.TransientDependency);
    }
    
    [Test]
    public void RegisteredByMethodToThreeInterfaces_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register(RegisterMethod);

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayerWithDependencies>();
        var secondValue = container.Resolve<IPlayerTwo>();
        var thirdValue = container.Resolve<IPlayerThree>();

        firstValue.Should().Be(secondValue);
        firstValue.Should().Be(thirdValue);

        firstValue.Should().BeOfType<PlayerWithDependencies>();
        secondValue.Should().BeOfType<PlayerWithDependencies>();
        thirdValue.Should().BeOfType<PlayerWithDependencies>();

        firstValue.SingletonDependency.Should().Be(secondValue.SingletonDependency);
        firstValue.SingletonDependency.Should().Be(thirdValue.SingletonDependency);

        firstValue.TransientDependency.Should().Be(secondValue.TransientDependency);
        firstValue.TransientDependency.Should().Be(thirdValue.TransientDependency);

        void RegisterMethod(IScopeBuilder scopeBuilder)
        {
            scopeBuilder.Register<IPlayerSingletonDependency, PlayerSingletonDependency>(Lifetime.Singleton);
            scopeBuilder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
            scopeBuilder.Register<IPlayerWithDependencies, IPlayerTwo, IPlayerThree, PlayerWithDependencies>(Lifetime.Singleton);
        }
    }
}