using FluentAssertions;
using NUnit.Framework;
using SparseInject;
using SparseInject.Tests.Scopes;
using SparseInject.Tests.Simple;

[TestFixture]
public class ScopeTests
{
    [Test]
    public void ScopeTest()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<GameController>();
            
        containerBuilder.RegisterScope<FeatureScope>(configurator =>
        {
            configurator.Register<IFeaturePopup, FeaturePopup>();
        });

        var container = containerBuilder.Build();

        var gameController = container.Resolve<GameController>();
            
        Assert.DoesNotThrow(gameController.Execute);
    }
    
    [Test]
    public void ScopeTest_2()
    {
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<GameController>();
        containerBuilder.Register<RewardService>();
            
        containerBuilder.RegisterScope<FeatureScope>(configurator =>
        {
            configurator.Register<IFeaturePopup, FeatureRewardPopup>();
        });

        var container = containerBuilder.Build();

        var gameController = container.Resolve<GameController>();
            
        Assert.DoesNotThrow(gameController.Execute);
    }
    
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