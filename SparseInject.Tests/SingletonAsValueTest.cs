using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class SingletonAsValueTest
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

    private class PlayerSingletonDependency : IPlayerSingletonDependency
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
    public void DependencyWithRegistration_WhenResolved_ReturnCorrectValueWithDependencies()
    {
        // Setup
        var builder = new ContainerBuilder();
        
        var playerSingletonDependency = new PlayerSingletonDependency();

        builder.RegisterValue<IPlayerSingletonDependency>(playerSingletonDependency);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        var value = container.Resolve<PlayerWithDependencies>();

        value.Should().BeOfType<PlayerWithDependencies>();
        value.TransientDependency.Should().BeOfType<PlayerTransientDependency>();
        value.SingletonDependency.Should().Be(playerSingletonDependency);
    }

    [Test]
    public void DependencyWithRegistrationToInterface_WhenResolved_ReturnCorrectValueWithDependencies()
    {
        // Setup
        var builder = new ContainerBuilder();
        
        var playerSingletonDependency = new PlayerSingletonDependency();

        builder.RegisterValue<IPlayerSingletonDependency>(playerSingletonDependency);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        var value = container.Resolve<IPlayerWithDependencies>();

        value.Should().BeOfType<PlayerWithDependencies>();
        value.SingletonDependency.Should().Be(playerSingletonDependency);
    }

    [Test]
    public void DependencyWithRegistrationToInterface_WhenResolvedNotInterface_ThrowProperException()
    {
        // Setup
        var builder = new ContainerBuilder();

        var playerSingletonDependency = new PlayerSingletonDependency();
        
        builder.RegisterValue<IPlayerSingletonDependency>(playerSingletonDependency);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        container.Invoking(subject => subject.Resolve<PlayerWithDependencies>())
            .Should()
            .Throw<SparseInjectException>();
    }

    [Test]
    public void DependencyWithRegistration_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        var playerSingletonDependency = new PlayerSingletonDependency();
        
        builder.RegisterValue<IPlayerSingletonDependency>(playerSingletonDependency);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<PlayerWithDependencies>();
        var secondValue = container.Resolve<PlayerWithDependencies>();

        firstValue.Should().Be(secondValue);
        firstValue.TransientDependency.Should().Be(secondValue.TransientDependency);

        firstValue.SingletonDependency.Should().Be(playerSingletonDependency);
        secondValue.SingletonDependency.Should().Be(playerSingletonDependency);
    }

    [Test]
    public void DependencyWithRegistrationToInterface_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        var playerSingletonDependency = new PlayerSingletonDependency();
        
        builder.RegisterValue<IPlayerSingletonDependency>(playerSingletonDependency);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayerWithDependencies>();
        var secondValue = container.Resolve<IPlayerWithDependencies>();
        var thirdValue = container.Resolve<IPlayerWithDependencies>();

        firstValue.Should().Be(secondValue);
        firstValue.Should().Be(thirdValue);

        firstValue.SingletonDependency.Should().Be(playerSingletonDependency);
        secondValue.SingletonDependency.Should().Be(playerSingletonDependency);
        thirdValue.SingletonDependency.Should().Be(playerSingletonDependency);

        firstValue.TransientDependency.Should().Be(secondValue.TransientDependency);
        firstValue.TransientDependency.Should().Be(thirdValue.TransientDependency);
    }

    [Test]
    public void DependencyWithRegistrationToTwoInterfaces_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();
        
        var playerSingletonDependency = new PlayerSingletonDependency();

        builder.RegisterValue<IPlayerSingletonDependency>(playerSingletonDependency);
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<IPlayerWithDependencies, IPlayerTwo, PlayerWithDependencies>(Lifetime.Singleton);

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayerWithDependencies>();
        var secondValue = container.Resolve<IPlayerTwo>();

        firstValue.Should().Be(secondValue);

        firstValue.SingletonDependency.Should().Be(playerSingletonDependency);
        secondValue.SingletonDependency.Should().Be(playerSingletonDependency);

        firstValue.TransientDependency.Should().Be(secondValue.TransientDependency);
    }

    [Test]
    public void DependencyWithRegistrationToThreeInterfaces_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        var playerSingletonDependency = new PlayerSingletonDependency();
        
        builder.RegisterValue<IPlayerSingletonDependency>(playerSingletonDependency);
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

        firstValue.SingletonDependency.Should().Be(playerSingletonDependency);
        secondValue.SingletonDependency.Should().Be(playerSingletonDependency);
        thirdValue.SingletonDependency.Should().Be(playerSingletonDependency);

        firstValue.TransientDependency.Should().Be(secondValue.TransientDependency);
        firstValue.TransientDependency.Should().Be(thirdValue.TransientDependency);
    }
    
    [Test]
    public void RegisteredToSelf_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        var value = new PlayerWithDependencies(
            Substitute.For<IPlayerSingletonDependency>(),
            Substitute.For<IPlayerTransientDependency>());
        
        builder.RegisterValue(value);

        var container = builder.Build();

        // Asserts
        container.Resolve<PlayerWithDependencies>().Should().Be(value);
        container.Resolve<PlayerWithDependencies>().Should().Be(value);
    }

    [Test]
    public void RegisteredToInterface_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        var value = new PlayerWithDependencies(
            Substitute.For<IPlayerSingletonDependency>(),
            Substitute.For<IPlayerTransientDependency>());

        builder.RegisterValue<IPlayerWithDependencies>(value);

        var container = builder.Build();

        // Asserts
        container.Resolve<IPlayerWithDependencies>().Should().Be(value);
        container.Resolve<IPlayerWithDependencies>().Should().Be(value);
    }
    
    [Test]
    public void RegisteredToInterfaceWithConcrete_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        var value = new PlayerWithDependencies(
            Substitute.For<IPlayerSingletonDependency>(),
            Substitute.For<IPlayerTransientDependency>());

        builder.RegisterValue<IPlayerWithDependencies, PlayerWithDependencies>(value);

        var container = builder.Build();

        // Asserts
        container.Resolve<IPlayerWithDependencies>().Should().Be(value);
        container.Resolve<IPlayerWithDependencies>().Should().Be(value);
    }
    
    [Test]
    public void RegisteredToTwoInterfaces_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();
        
        var value = new PlayerWithDependencies(
            Substitute.For<IPlayerSingletonDependency>(),
            Substitute.For<IPlayerTransientDependency>());

        builder.RegisterValue<IPlayerWithDependencies, IPlayerTwo, PlayerWithDependencies>(value);

        var container = builder.Build();

        // Asserts
        container.Resolve<IPlayerWithDependencies>().Should().Be(value);
        container.Resolve<IPlayerWithDependencies>().Should().Be(value);
        
        container.Resolve<IPlayerTwo>().Should().Be(value);
        container.Resolve<IPlayerTwo>().Should().Be(value);
    }

    [Test]
    public void RegisteredToThreeInterfaces_WhenResolvedMultipleTimes_ReturnSameValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        var value = new PlayerWithDependencies(
            Substitute.For<IPlayerSingletonDependency>(),
            Substitute.For<IPlayerTransientDependency>());

        builder.RegisterValue<IPlayerWithDependencies, IPlayerTwo, IPlayerThree, PlayerWithDependencies>(value);

        var container = builder.Build();

        // Asserts
        container.Resolve<IPlayerWithDependencies>().Should().Be(value);
        container.Resolve<IPlayerWithDependencies>().Should().Be(value);
        
        container.Resolve<IPlayerTwo>().Should().Be(value);
        container.Resolve<IPlayerTwo>().Should().Be(value);
        
        container.Resolve<IPlayerThree>().Should().Be(value);
        container.Resolve<IPlayerThree>().Should().Be(value);
    }

    [Test]
    public void RegisteredToTwoInterfaces_WhenUsingWrongSignature_ThrowProperException()
    {
        // Setup
        var builder = new ContainerBuilder();
        
        var value = new PlayerWithDependencies(
            Substitute.For<IPlayerSingletonDependency>(),
            Substitute.For<IPlayerTransientDependency>());

        // Asserts
        builder.Invoking(subject => builder.RegisterValue<IPlayerWithDependencies, IPlayerTwo>(value))
            .Should().Throw<SparseInjectException>();
    }
}