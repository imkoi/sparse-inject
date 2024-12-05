using SparseInject.Tests.Collection;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class CollectionTests
{
    [Test]
    public void TransientBinding()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<PlayerController>();
        containerBuilder.Register<IPlayerControllerProcessor, PlayerMovementProcessor>();
        containerBuilder.Register<IPlayerControllerProcessor, PlayerShootingProcessor>();
        containerBuilder.Register<IPlayerControllerProcessor, PlayerAnimationProcessor>();

        var container = containerBuilder.Build();
        var processor = container.Resolve<IPlayerControllerProcessor>();
        var processors = container.Resolve<IPlayerControllerProcessor[]>();
        var controller = container.Resolve<PlayerController>();

        processors.Length.Should().Be(3);
    }

    [Test]
    public void UnorderedBindings()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<IPlayerControllerProcessor, PlayerMovementProcessor>();
        containerBuilder.Register<IPlayerControllerProcessor, PlayerAnimationProcessor>();

        containerBuilder.Register<PlayerController>();

        containerBuilder.Register<IPlayerControllerProcessor, PlayerShootingProcessor>();

        var container = containerBuilder.Build();
        var processors = container.Resolve<IPlayerControllerProcessor[]>();

        processors.Length.Should().Be(3);
    }
}