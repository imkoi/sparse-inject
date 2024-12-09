using System;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;
using SparseInject.ReflectionBaking.Tests.PartialBaking;
using SparseInject.Tests.Simple;

[TestFixture]
public class ExceptionTest
{
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