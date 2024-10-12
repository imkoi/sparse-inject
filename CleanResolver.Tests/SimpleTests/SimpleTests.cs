using CleanResolver;
using CleanResolver.Tests.Simple;
using FluentAssertions;
using NUnit.Framework;

[TestFixture]
public class SimpleTests
{
    [Test]
    public void UnorderedWithSparseRewireBindings()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<IPlayerControllerProcessor, PlayerMovementProcessor>();
        containerBuilder.Register<IPlayerControllerProcessor, PlayerAnimationProcessor>();

        containerBuilder.Register<PlayerController>();

        containerBuilder.Register<IPlayerControllerProcessor, PlayerShootingProcessor>();
   
        containerBuilder.Register<PlayerService>();

        var container = containerBuilder.Build();
        var processors = container.Resolve<PlayerService>();

        processors.Should().NotBeNull();
    }
    
    [Test]
    public void UnorderedWithSparseRewireBindings_2()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<IAudioManager, AudioManager>();
        containerBuilder.Register<IPlayerControllerProcessor, PlayerMovementProcessor>();
        containerBuilder.Register<IPlayerControllerProcessor, PlayerAnimationProcessor>();

        containerBuilder.Register<PlayerController>();

        containerBuilder.Register<IPlayerControllerProcessor, PlayerShootingProcessor>();
        containerBuilder.Register<IAudioManager, PlayerAudioManager>();
   
        containerBuilder.Register<PlayerService>();

        var container = containerBuilder.Build();
        var processors = container.Resolve<PlayerService>();

        processors.Should().NotBeNull();
    }
}