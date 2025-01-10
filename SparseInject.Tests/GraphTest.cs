using System;
using System.Linq;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using SparseInject;
using SparseInject.ReflectionBaking.Tests.PartialBaking;

public class GraphTest
{
    private class DependencyA : IDisposable
    {
        public void Dispose()
        {
            
        }
    }
    
    private class DependencyB : IDisposable
    {
        public void Dispose()
        {
            
        }
    }
    
    [Test]
    public void RegisterJaggedAndSimpleCollection_WhenResolveOneRankCollection_GraphBuiltProperly()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterValue<IDisposable[]>(new DependencyA[2]
        {
            new DependencyA(),
            new DependencyA()
        });
        
        var containerBuilderGraph = containerBuilder.GetGraph();

        containerBuilderGraph.ParentContainers.Should().BeEmpty();
        containerBuilderGraph.Contracts.Count.Should().Be(2);
        
        containerBuilderGraph.Contracts[0].Type.Should().Be(typeof(IDisposable));
        containerBuilderGraph.Contracts[0].IsCollection.Should().BeTrue();
        containerBuilderGraph.Contracts[0].Concretes.Count.Should().Be(1);
        containerBuilderGraph.Contracts[0].Concretes[0].Type.Should().Be(typeof(IDisposable[]));
        containerBuilderGraph.Contracts[0].Concretes[0].IsSingleton.Should().BeTrue();
        containerBuilderGraph.Contracts[0].Concretes[0].IsArray.Should().BeTrue();
        containerBuilderGraph.Contracts[0].Concretes[0].IsScope.Should().BeFalse();
        containerBuilderGraph.Contracts[0].Concretes[0].IsFactory.Should().BeFalse();
        
        containerBuilderGraph.Contracts[1].Type.Should().Be(typeof(IDisposable[]));
        containerBuilderGraph.Contracts[1].IsCollection.Should().BeTrue();
        containerBuilderGraph.Contracts[1].Concretes.Count.Should().Be(1);

        containerBuilderGraph.Contracts[1].Concretes[0].Should().BeEquivalentTo(
            containerBuilderGraph.Contracts[0].Concretes[0]);
        
        containerBuilder.RegisterValue<IDisposable[][]>(new IDisposable[2][]
        {
            new DependencyA[2]
            {
                new DependencyA(),
                new DependencyA()
            },
            new DependencyB[2]
            {
                new DependencyB(),
                new DependencyB()
            }
        });
        
        var container = containerBuilder.Build();

        var containerGraph = container.GetGraph();

        containerGraph.ParentContainers.Should().BeEmpty();
        containerGraph.Contracts.Count.Should().Be(3);
        
        containerGraph.Contracts[0].Type.Should().Be(typeof(IDisposable));
        containerGraph.Contracts[0].IsCollection.Should().BeTrue();
        containerGraph.Contracts[0].Concretes.Count.Should().Be(1);
        containerGraph.Contracts[0].Concretes[0].Type.Should().Be(typeof(IDisposable[]));
        containerGraph.Contracts[0].Concretes[0].Dependencies.Count.Should().Be(0);
        containerGraph.Contracts[0].Concretes[0].IsSingleton.Should().BeTrue();
        containerGraph.Contracts[0].Concretes[0].IsArray.Should().BeTrue();
        containerGraph.Contracts[0].Concretes[0].IsScope.Should().BeFalse();
        containerGraph.Contracts[0].Concretes[0].IsFactory.Should().BeFalse();
        
        containerGraph.Contracts[1].Type.Should().Be(typeof(IDisposable[]));
        containerGraph.Contracts[1].IsCollection.Should().BeTrue();
        containerGraph.Contracts[1].Concretes.Count.Should().Be(2);

        containerGraph.Contracts[1].Concretes[0].Should().BeEquivalentTo(
            containerGraph.Contracts[0].Concretes[0]);
        
        containerGraph.Contracts[1].Concretes[1].Type.Should().Be(typeof(IDisposable[][]));
        containerGraph.Contracts[1].Concretes[1].Dependencies.Count.Should().Be(0);
        containerGraph.Contracts[1].Concretes[1].IsSingleton.Should().BeTrue();
        containerGraph.Contracts[1].Concretes[1].IsArray.Should().BeTrue();
        containerGraph.Contracts[1].Concretes[1].IsScope.Should().BeFalse();
        containerGraph.Contracts[1].Concretes[1].IsFactory.Should().BeFalse();
        
        containerGraph.Contracts[2].Type.Should().Be(typeof(IDisposable[][]));
        containerGraph.Contracts[2].IsCollection.Should().BeTrue();
        containerGraph.Contracts[2].Concretes.Count.Should().Be(1);

        containerGraph.Contracts[2].Concretes[0].Should().BeEquivalentTo(containerGraph.Contracts[1].Concretes[1]);
    }
    
    private class ScopeA : Scope { }
    private class ScopeB : Scope { }
    private class ScopeC : Scope { }
    private class MainScopeDependency { }
    
    [Test]
    public void RegisteredInnerScopes_WhenLastScopeResolveInstanceFromMain_GraphBuiltProperly()
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.Register<MainScopeDependency>();
        
        var containerBuilderGraph = containerBuilder.GetGraph();
        
        containerBuilder.RegisterScope<ScopeA>(configuratorA =>
        {
            configuratorA.RegisterScope<ScopeB>(configuratorB =>
            {
                configuratorB.RegisterScope<ScopeC>(_ => { });
                
                var configuratorGraph = (configuratorB as ContainerBuilder).GetGraph();
                
                configuratorGraph.ParentContainers.Count.Should().Be(1);
                configuratorGraph.ParentContainers.First().Contracts.Count.Should().Be(2);
            });
        });

        var container = containerBuilder.Build();
        
        var containerGraph = container.GetGraph();
        
        // ContainerBuilder graph asserts
        containerBuilderGraph.ParentContainers.Should().BeEmpty();
        containerBuilderGraph.Contracts.Count.Should().Be(2);
        
        containerBuilderGraph.Contracts[0].Type.Should().Be(typeof(MainScopeDependency));
        containerBuilderGraph.Contracts[0].IsCollection.Should().BeFalse();
        containerBuilderGraph.Contracts[0].Concretes.Count.Should().Be(1);
        containerBuilderGraph.Contracts[0].Concretes[0].Type.Should().Be(typeof(MainScopeDependency));
        containerBuilderGraph.Contracts[0].Concretes[0].IsSingleton.Should().BeFalse();
        containerBuilderGraph.Contracts[0].Concretes[0].IsArray.Should().BeFalse();
        containerBuilderGraph.Contracts[0].Concretes[0].IsScope.Should().BeFalse();
        containerBuilderGraph.Contracts[0].Concretes[0].IsFactory.Should().BeFalse();
        
        containerBuilderGraph.Contracts[1].Type.Should().Be(typeof(MainScopeDependency));
        containerBuilderGraph.Contracts[1].IsCollection.Should().BeTrue();
        containerBuilderGraph.Contracts[1].Concretes.Count.Should().Be(1);

        containerBuilderGraph.Contracts[1].Concretes[0].Should().BeEquivalentTo(
            containerBuilderGraph.Contracts[0].Concretes[0]);
        
        // Container graph asserts
        containerGraph.ParentContainers.Should().BeEmpty();
        containerGraph.Contracts.Count.Should().Be(4);
        
        containerGraph.Contracts[0].Type.Should().Be(typeof(MainScopeDependency));
        containerGraph.Contracts[0].IsCollection.Should().BeFalse();
        containerGraph.Contracts[0].Concretes.Count.Should().Be(1);
        containerGraph.Contracts[0].Concretes[0].Type.Should().Be(typeof(MainScopeDependency));
        containerGraph.Contracts[0].Concretes[0].Dependencies.Count.Should().Be(0);
        containerGraph.Contracts[0].Concretes[0].IsSingleton.Should().BeFalse();
        containerGraph.Contracts[0].Concretes[0].IsArray.Should().BeFalse();
        containerGraph.Contracts[0].Concretes[0].IsScope.Should().BeFalse();
        containerGraph.Contracts[0].Concretes[0].IsFactory.Should().BeFalse();
        
        containerGraph.Contracts[1].Type.Should().Be(typeof(MainScopeDependency));
        containerGraph.Contracts[1].IsCollection.Should().BeTrue();
        containerGraph.Contracts[1].Concretes.Count.Should().Be(1);

        containerGraph.Contracts[1].Concretes[0].Should().BeEquivalentTo(
            containerGraph.Contracts[0].Concretes[0]);
        
        containerGraph.Contracts[2].Type.Should().Be(typeof(ScopeA));
        containerGraph.Contracts[2].IsCollection.Should().BeFalse();
        containerGraph.Contracts[2].Concretes.Count.Should().Be(1);
        containerGraph.Contracts[2].Concretes[0].Type.Should().Be(typeof(ScopeA));
        containerGraph.Contracts[2].Concretes[0].Dependencies.Count.Should().Be(0);
        containerGraph.Contracts[2].Concretes[0].IsSingleton.Should().BeFalse();
        containerGraph.Contracts[2].Concretes[0].IsArray.Should().BeFalse();
        containerGraph.Contracts[2].Concretes[0].IsScope.Should().BeTrue();
        containerGraph.Contracts[2].Concretes[0].IsFactory.Should().BeFalse();
        
        containerGraph.Contracts[3].Type.Should().Be(typeof(ScopeA));
        containerGraph.Contracts[3].IsCollection.Should().BeTrue();
        containerGraph.Contracts[3].Concretes.Count.Should().Be(1);

        containerGraph.Contracts[3].Concretes[0].Should().BeEquivalentTo(
            containerGraph.Contracts[2].Concretes[0]);

        var scopeA = container.Resolve<ScopeA>();
        var scopeB = scopeA._container.Resolve<ScopeB>();
        var scopeC = scopeB._container.Resolve<ScopeC>();
        
        containerGraph = scopeC._container.GetGraph();
        containerGraph.Contracts.Count.Should().Be(0);
        containerGraph.ParentContainers.Count.Should().Be(1);
        containerGraph.ParentContainers[0].Contracts.Count.Should().Be(2);
        containerGraph.ParentContainers[0].Contracts[0].Type.Should().Be(typeof(ScopeC));
        containerGraph.ParentContainers[0].Contracts[0].IsCollection.Should().BeFalse();
        containerGraph.ParentContainers[0].Contracts[0].Concretes.Count.Should().Be(1);
        containerGraph.ParentContainers[0].Contracts[1].Type.Should().Be(typeof(ScopeC));
        containerGraph.ParentContainers[0].Contracts[1].IsCollection.Should().BeTrue();
        containerGraph.ParentContainers[0].Contracts[1].Concretes.Count.Should().Be(1);
        
        scopeC._container.Resolve<MainScopeDependency>().Should().BeOfType<MainScopeDependency>();
    }
    
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

    private interface IPlayer { }
    private interface IPlayerTwo : IPlayer { }
    private interface IPlayerThree : IPlayer { }
    
    [Test]
    public void RegisteredToThreeInterfaces_WhenResolvedMultipleTimes_GraphBuiltProperly()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.Register<IPlayer, IPlayerTwo, IPlayerThree, Player>(Lifetime.Singleton);

        var containerBuilderGraph = builder.GetGraph();

        var container = builder.Build();

        var containerGraph = container.GetGraph();

        // Asserts
        containerBuilderGraph.Contracts.Count.Should().Be(6);
        containerBuilderGraph.Contracts[0].Type.Should().Be(typeof(IPlayer));
        containerBuilderGraph.Contracts[0].IsCollection.Should().BeFalse();
        containerBuilderGraph.Contracts[0].Concretes.Count.Should().Be(1);
        containerBuilderGraph.Contracts[1].Type.Should().Be(typeof(IPlayer));
        containerBuilderGraph.Contracts[1].IsCollection.Should().BeTrue();
        containerBuilderGraph.Contracts[1].Concretes.Count.Should().Be(1);
        
        containerBuilderGraph.Contracts[2].Type.Should().Be(typeof(IPlayerTwo));
        containerBuilderGraph.Contracts[2].IsCollection.Should().BeFalse();
        containerBuilderGraph.Contracts[2].Concretes.Count.Should().Be(1);
        containerBuilderGraph.Contracts[3].Type.Should().Be(typeof(IPlayerTwo));
        containerBuilderGraph.Contracts[3].IsCollection.Should().BeTrue();
        containerBuilderGraph.Contracts[3].Concretes.Count.Should().Be(1);
        
        containerBuilderGraph.Contracts[4].Type.Should().Be(typeof(IPlayerThree));
        containerBuilderGraph.Contracts[4].IsCollection.Should().BeFalse();
        containerBuilderGraph.Contracts[4].Concretes.Count.Should().Be(1);
        containerBuilderGraph.Contracts[5].Type.Should().Be(typeof(IPlayerThree));
        containerBuilderGraph.Contracts[5].IsCollection.Should().BeTrue();
        containerBuilderGraph.Contracts[5].Concretes.Count.Should().Be(1);
        
        var allConcretes = containerBuilderGraph.Contracts
            .Select(contract => contract.Concretes.First())
            .ToArray();
        var referenceConcrete = allConcretes.First();

        foreach (var concrete in allConcretes)
        {
            concrete.Should().BeEquivalentTo(referenceConcrete);
        }
    }
    
    private class PlayerFromFactory : IPlayerFromFactory
    {
        public int MaxHealth { get; set; }
        public IAudioService AudioService { get; set; }
    }

    private interface IPlayerFromFactory
    {
        int MaxHealth { get; }
        IAudioService AudioService { get; }
    }

    public interface IAudioService { }
    
    [Test]
    public void RegisteredFactoryWithContainerAndParameter_WhenResolvedAndInvoked_GraphBuiltProperly()
    {
        // Setup
        var builder = new ContainerBuilder();
        var audioService = Substitute.For<IAudioService>();

        builder.RegisterValue(audioService);
        builder.RegisterFactory<int, IPlayerFromFactory>(container => maxHealth => new PlayerFromFactory
        {
            MaxHealth = maxHealth,
            AudioService = container.Resolve<IAudioService>()
        });
        
        var containerBuilderGraph = builder.GetGraph();
        
        containerBuilderGraph.Contracts[2].Concretes[0].IsFactory.Should().BeTrue();
        containerBuilderGraph.Contracts[3].Concretes[0].IsFactory.Should().BeTrue();

        var container = builder.Build();

        var containerGraph = container.GetGraph();
        
        containerGraph.Contracts[2].Concretes[0].IsFactory.Should().BeTrue();
        containerGraph.Contracts[3].Concretes[0].IsFactory.Should().BeTrue();
    }
    
    [Ignore("Graphs dont provide is contract use reflection baking or not YET")]
    [Test]
    public void RegisterApi_WhenResolving_GraphBuiltProperly()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register(PartialBakingTestInstaller.Install);
        
        var container = containerBuilder.Build();
    }
    
    private class PlayerWithDependencies : IPlayerWithDependencies, IPlayerTwoWithDependencies, IPlayerThreeWithDependencies
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

    private interface IPlayerWithDependencies
    {
        IPlayerSingletonDependency SingletonDependency { get; }
        IPlayerTransientDependency TransientDependency { get; }
    }

    private interface IPlayerTwoWithDependencies : IPlayerWithDependencies
    {
        IPlayerSingletonDependency SingletonDependency { get; }
        IPlayerTransientDependency TransientDependency { get; }
    }

    private interface IPlayerThreeWithDependencies : IPlayerWithDependencies
    {
        IPlayerSingletonDependency SingletonDependency { get; }
        IPlayerTransientDependency TransientDependency { get; }
    }

    public interface IPlayerTransientDependency { }
    public interface IPlayerSingletonDependency { }

    [Test]
    public void RegisteredTransientWithDependencies_WhenResolved_GraphBuiltProperly()
    {
        // Setup
        var builder = new ContainerBuilder();

        builder.RegisterValue(Substitute.For<IPlayerSingletonDependency>());
        builder.Register<IPlayerTransientDependency, PlayerTransientDependency>();
        builder.Register<PlayerWithDependencies>();

        var containerBuilderGraph = builder.GetGraph();

        var container = builder.Build();
        
        var containerGraph = container.GetGraph();

        // Assert builder graph
        containerBuilderGraph.Contracts.Count.Should().Be(6);
        
        containerBuilderGraph.Contracts[0].Type.Should().Be(typeof(IPlayerSingletonDependency));
        containerBuilderGraph.Contracts[0].IsCollection.Should().BeFalse();
        containerBuilderGraph.Contracts[0].Concretes.Count.Should().Be(1);
        containerBuilderGraph.Contracts[1].Type.Should().Be(typeof(IPlayerSingletonDependency));
        containerBuilderGraph.Contracts[1].IsCollection.Should().BeTrue();
        containerBuilderGraph.Contracts[1].Concretes.Count.Should().Be(1);
        containerBuilderGraph.Contracts[1].Concretes.Should()
            .BeEquivalentTo(containerBuilderGraph.Contracts[0].Concretes);
        
        containerBuilderGraph.Contracts[2].Type.Should().Be(typeof(IPlayerTransientDependency));
        containerBuilderGraph.Contracts[2].IsCollection.Should().BeFalse();
        containerBuilderGraph.Contracts[2].Concretes.Count.Should().Be(1);
        containerBuilderGraph.Contracts[3].Type.Should().Be(typeof(IPlayerTransientDependency));
        containerBuilderGraph.Contracts[3].IsCollection.Should().BeTrue();
        containerBuilderGraph.Contracts[3].Concretes.Count.Should().Be(1);
        containerBuilderGraph.Contracts[3].Concretes.Should()
            .BeEquivalentTo(containerBuilderGraph.Contracts[2].Concretes);
        
        containerBuilderGraph.Contracts[4].Type.Should().Be(typeof(PlayerWithDependencies));
        containerBuilderGraph.Contracts[4].IsCollection.Should().BeFalse();
        containerBuilderGraph.Contracts[4].Concretes.Count.Should().Be(1);
        containerBuilderGraph.Contracts[5].Type.Should().Be(typeof(PlayerWithDependencies));
        containerBuilderGraph.Contracts[5].IsCollection.Should().BeTrue();
        containerBuilderGraph.Contracts[5].Concretes.Count.Should().Be(1);
        containerBuilderGraph.Contracts[5].Concretes.Should()
            .BeEquivalentTo(containerBuilderGraph.Contracts[4].Concretes);
        
        // Assert container graph
        containerGraph.Contracts.Count.Should().Be(6);
        
        containerGraph.Contracts[0].Type.Should().Be(typeof(IPlayerSingletonDependency));
        containerGraph.Contracts[0].IsCollection.Should().BeFalse();
        containerGraph.Contracts[0].Concretes.Count.Should().Be(1);
        containerGraph.Contracts[1].Type.Should().Be(typeof(IPlayerSingletonDependency));
        containerGraph.Contracts[1].IsCollection.Should().BeTrue();
        containerGraph.Contracts[1].Concretes.Count.Should().Be(1);
        containerGraph.Contracts[1].Concretes.Should()
            .BeEquivalentTo(containerBuilderGraph.Contracts[0].Concretes);
        
        containerGraph.Contracts[2].Type.Should().Be(typeof(IPlayerTransientDependency));
        containerGraph.Contracts[2].IsCollection.Should().BeFalse();
        containerGraph.Contracts[2].Concretes.Count.Should().Be(1);
        containerGraph.Contracts[3].Type.Should().Be(typeof(IPlayerTransientDependency));
        containerGraph.Contracts[3].IsCollection.Should().BeTrue();
        containerGraph.Contracts[3].Concretes.Count.Should().Be(1);
        containerGraph.Contracts[3].Concretes.Should()
            .BeEquivalentTo(containerBuilderGraph.Contracts[2].Concretes);
        
        containerGraph.Contracts[4].Type.Should().Be(typeof(PlayerWithDependencies));
        containerGraph.Contracts[4].IsCollection.Should().BeFalse();
        containerGraph.Contracts[4].Concretes.Count.Should().Be(1);
        containerGraph.Contracts[5].Type.Should().Be(typeof(PlayerWithDependencies));
        containerGraph.Contracts[5].IsCollection.Should().BeTrue();
        containerGraph.Contracts[5].Concretes.Count.Should().Be(1);

        containerGraph.Contracts[5].Concretes.Should()
            .BeEquivalentTo(containerGraph.Contracts[4].Concretes);

        var concretes = containerGraph.Contracts[5].Concretes;
        
        concretes.Count.Should().Be(1);
        concretes.First().Dependencies.Count.Should().Be(2);

        concretes[0].Dependencies.Contains(containerGraph.Contracts[0]).Should().BeTrue();
        concretes[0].Dependencies.Contains(containerGraph.Contracts[2]).Should().BeTrue();
    }
}