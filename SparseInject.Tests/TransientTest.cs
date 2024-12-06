using FluentAssertions;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class TransientTest
{
    private const int DefaultMaxHealth = 100;

    private class Player : IPlayer, IPlayerTwo, IPlayerThree
    {
        public int DefaultValueInitializedThroughConstructor { get; }
        public int DefaultValueInitializedThroughProperty { get; } = DefaultMaxHealth;

        public Player()
        {
            DefaultValueInitializedThroughConstructor = DefaultMaxHealth;
        }
    }

    private interface IPlayer
    {
        public int DefaultValueInitializedThroughConstructor { get; }
        public int DefaultValueInitializedThroughProperty { get; }
    }

    private interface IPlayerTwo : IPlayer
    {
    }

    private interface IPlayerThree : IPlayer
    {
    }

    [Test]
    public void Registered_WhenResolved_ReturnCorrectValue()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<Player>();

        var container = builder.Build();

        // Asserts
        var value = container.Resolve<Player>();

        value.Should().BeOfType<Player>();
    }

    [Test]
    public void Registered_WhenResolved_DefaultConstructorIsInvoked()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<Player>();

        var container = builder.Build();

        // Asserts
        var value = container.Resolve<Player>();

        value.DefaultValueInitializedThroughConstructor.Should().Be(DefaultMaxHealth);
    }

    [Test]
    public void Registered_WhenResolved_DefaultMembersIsInitialized()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<Player>();

        var container = builder.Build();

        // Asserts
        var value = container.Resolve<Player>();

        value.DefaultValueInitializedThroughProperty.Should().Be(DefaultMaxHealth);
    }

    [Test]
    public void NotRegistered_WhenResolved_ThrowProperException()
    {
        // Setup
        var builder = new ContainerBuilder();
        var container = builder.Build();

        // Asserts
        container.Invoking(subject => subject.Resolve<Player>())
            .Should()
            .Throw<SparseInjectException>();
    }

    [Test]
    public void RegisteredToInterface_WhenResolved_ReturnCorrectValue()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayer, Player>();

        var container = builder.Build();

        // Asserts
        var value = container.Resolve<IPlayer>();

        value.Should().BeOfType<Player>();
    }

    [Test]
    public void RegisteredToInterface_WhenResolvedNotInterface_ThrowProperException()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayer, Player>();

        var container = builder.Build();

        // Asserts
        container.Invoking(subject => subject.Resolve<Player>())
            .Should()
            .Throw<SparseInjectException>();
    }

    [Test]
    public void Registered_WhenResolvedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<Player>();

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<Player>();
        var secondValue = container.Resolve<Player>();

        firstValue.Should().NotBe(secondValue);
    }

    [Test]
    public void RegisteredToInterface_WhenResolvedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayer, Player>();

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayer>();
        var secondValue = container.Resolve<IPlayer>();

        firstValue.Should().NotBe(secondValue);
    }

    [Test]
    public void RegisteredToTwoInterfaces_WhenResolvedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayer, IPlayerTwo, Player>();

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayer>();
        var secondValue = container.Resolve<IPlayerTwo>();

        firstValue.Should().BeOfType<Player>();
        secondValue.Should().BeOfType<Player>();

        firstValue.Should().NotBe(secondValue);
    }

    [Test]
    public void RegisteredToThreeInterfaces_WhenResolvedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayer, IPlayerTwo, IPlayerThree, Player>();

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayer>();
        var secondValue = container.Resolve<IPlayerTwo>();
        var thirdValue = container.Resolve<IPlayerThree>();

        firstValue.Should().BeOfType<Player>();
        secondValue.Should().BeOfType<Player>();
        thirdValue.Should().BeOfType<Player>();

        firstValue.Should().NotBe(secondValue);
        firstValue.Should().NotBe(thirdValue);
    }
    
    [Test]
    public void RegisteredByMethodToThreeInterfaces_WhenResolvedMultipleTimes_ReturnDifferentValues()
    {
        // Setup
        var builder = new ContainerBuilder();
        
        builder.Register(scopeBuilder =>
        {
            scopeBuilder.Register<IPlayer, IPlayerTwo, IPlayerThree, Player>();
        });

        var container = builder.Build();

        // Asserts
        var firstValue = container.Resolve<IPlayer>();
        var secondValue = container.Resolve<IPlayerTwo>();
        var thirdValue = container.Resolve<IPlayerThree>();

        firstValue.Should().BeOfType<Player>();
        secondValue.Should().BeOfType<Player>();
        thirdValue.Should().BeOfType<Player>();

        firstValue.Should().NotBe(secondValue);
        firstValue.Should().NotBe(thirdValue);
    }
}