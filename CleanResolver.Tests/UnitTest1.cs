using CleanResolver.Tests.TestSources;
using FluentAssertions;
using NUnit.Framework;

namespace CleanResolver.Tests;

public class Tests
{
    private ContainerBuilder _containerBuilder;
    
    [SetUp]
    public void Setup()
    {
        _containerBuilder = new ContainerBuilder();
    }

    [Test]
    public void TransientBinding()
    {
        _containerBuilder.Register<IPlayerControllerProcessor, PlayerMovementProcessor>();
        _containerBuilder.Register<IPlayerControllerProcessor, PlayerShootingProcessor>();
        _containerBuilder.Register<IPlayerControllerProcessor, PlayerAnimationProcessor>();
        
        _containerBuilder.Register<IPlayerView, PlayerView>();
        _containerBuilder.Register<IPlayerModel, PlayerModel>();
        
        _containerBuilder.Register<ITickable, PlayerController>();
        _containerBuilder.Register<ITickable, WeaponController>();

        var container = _containerBuilder.Build();

        foreach (var tickable in container.Resolve<ITickable[]>())
        {
            tickable.Tick(0f);
        }

        Console.WriteLine(_containerBuilder);
    }
    
    [Test]
    public void UnorderedBindings()
    {
        _containerBuilder.Register<IPlayerControllerProcessor, PlayerMovementProcessor>();
        _containerBuilder.Register<IPlayerControllerProcessor, PlayerAnimationProcessor>();
        
        _containerBuilder.Register<IPlayerView, PlayerView>();
        _containerBuilder.Register<IPlayerModel, PlayerModel>();
        
        _containerBuilder.Register<ITickable, PlayerController>();
        _containerBuilder.Register<IPlayerControllerProcessor, PlayerShootingProcessor>();
        _containerBuilder.Register<ITickable, WeaponController>();

        var container = _containerBuilder.Build();

        foreach (var tickable in container.Resolve<ITickable[]>())
        {
            tickable.Tick(0f);
        }

        Console.WriteLine(_containerBuilder);
    }
    
    [Test]
    public void ManyBindings()
    {
        ContainerBinder.BindDeps(_containerBuilder);

        var container = _containerBuilder.Build();
        
        var highestDependency = container.Resolve<Class0>();
        
        highestDependency.Should().NotBeNull();
    }
}