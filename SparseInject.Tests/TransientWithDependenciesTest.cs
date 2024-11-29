using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class TransientWithDependenciesTest
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

    private class PlayerTransientDependency : IPlayerTransientDependency
    {
    }

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

    public interface IPlayerTransientDependency
    {
    }

    public interface IPlayerSingletonDependency
    {
    }

    [Test]
    public void Registered_WhenResolved_ReturnCorrectValueWithDependencies()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IPlayerSingletonDependency>());
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<PlayerWithDependencies>();

        var container = builder.Build();

        // Asserts
        var value = container.Resolve<PlayerWithDependencies>();

        value.Should().BeOfType<PlayerWithDependencies>();
        value.TransientDependency.Should().BeOfType<PlayerTransientDependency>();
        value.SingletonDependency.Should().NotBeNull();
    }

    [Test]
    public void RegisteredToInterface_WhenResolved_ReturnCorrectValueWithDependencies()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IPlayerSingletonDependency>());
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, PlayerWithDependencies>();

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

        builder.Register<PlayerWithDependencies>();

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

        builder.RegisterValue(Substitute.For<IPlayerSingletonDependency>());
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, PlayerWithDependencies>();

        var container = builder.Build();

        // Asserts
        container.Invoking(subject => subject.Resolve<PlayerWithDependencies>())
            .Should()
            .Throw<SparseInjectException>();
    }

    [Test]
    public void Registered_WhenResolvedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IPlayerSingletonDependency>());
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<PlayerWithDependencies>();

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<PlayerWithDependencies>();
        var secondValue = container.Resolve<PlayerWithDependencies>();

        firstValue.Should().NotBe(secondValue);
        firstValue.SingletonDependency.Should().Be(secondValue.SingletonDependency);
        firstValue.TransientDependency.Should().NotBe(secondValue.TransientDependency);
    }

    [Test]
    public void RegisteredToInterface_WhenResolvedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IPlayerSingletonDependency>());
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, PlayerWithDependencies>();

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayerWithDependencies>();
        var secondValue = container.Resolve<IPlayerWithDependencies>();
        var thirdValue = container.Resolve<IPlayerWithDependencies>();

        firstValue.Should().NotBe(secondValue);
        firstValue.Should().NotBe(thirdValue);

        firstValue.SingletonDependency.Should().Be(secondValue.SingletonDependency);
        firstValue.SingletonDependency.Should().Be(thirdValue.SingletonDependency);

        firstValue.TransientDependency.Should().NotBe(secondValue.TransientDependency);
        firstValue.TransientDependency.Should().NotBe(thirdValue.TransientDependency);
    }

    [Test]
    public void RegisteredToTwoInterfaces_WhenResolvedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IPlayerSingletonDependency>());
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, IPlayerTwo, PlayerWithDependencies>();

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayerWithDependencies>();
        var secondValue = container.Resolve<IPlayerTwo>();

        firstValue.Should().NotBe(secondValue);

        firstValue.SingletonDependency.Should().Be(secondValue.SingletonDependency);

        firstValue.TransientDependency.Should().NotBe(secondValue.TransientDependency);
    }

    [Test]
    public void RegisteredToThreeInterfaces_WhenResolvedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IPlayerSingletonDependency>());
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, IPlayerTwo, IPlayerThree, PlayerWithDependencies>();

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayerWithDependencies>();
        var secondValue = container.Resolve<IPlayerTwo>();
        var thirdValue = container.Resolve<IPlayerThree>();

        firstValue.Should().NotBe(secondValue);
        firstValue.Should().NotBe(thirdValue);

        firstValue.Should().BeOfType<PlayerWithDependencies>();
        secondValue.Should().BeOfType<PlayerWithDependencies>();
        thirdValue.Should().BeOfType<PlayerWithDependencies>();

        firstValue.SingletonDependency.Should().Be(secondValue.SingletonDependency);
        firstValue.SingletonDependency.Should().Be(thirdValue.SingletonDependency);

        firstValue.TransientDependency.Should().NotBe(secondValue.TransientDependency);
        firstValue.TransientDependency.Should().NotBe(thirdValue.TransientDependency);
    }
}