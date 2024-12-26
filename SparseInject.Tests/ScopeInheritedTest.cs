using FluentAssertions;
using NUnit.Framework;
using SparseInject;
using SparseInject.Tests.Scopes;
using SparseInject.Tests.Simple;

[TestFixture]
public class ScopeInheritedTest
{
    private class ScopeA : Scope
    {
    }

    [Test]
    public void RegisterEmptyScope_WhenResolve_CanBeResolved()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterScope<ScopeA>(configurator => { });

        var container = containerBuilder.Build();

        // Asserts
        var scope = container.Resolve<ScopeA>();

        scope.Should().BeOfType<ScopeA>();
    }
    
    private class ScopeBDependency { }

    private class ScopeB : Scope
    {
        public ScopeBDependency Dependency { get; }
        
        public ScopeB(ScopeBDependency dependency)
        {
            Dependency = dependency;
        }
    }
    
    [Test]
    public void RequestedDependency_WhenNotExistInCurrentScope_ShouldTakeFromMainScope()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<ScopeBDependency>();
        containerBuilder.RegisterScope<ScopeB>(configurator => { });

        var container = containerBuilder.Build();

        // Asserts
        var scope = container.Resolve<ScopeB>();

        scope.Dependency.Should().BeOfType<ScopeBDependency>();
    }
    
    [Test]
    public void RequestedDependency_WhenExistInCurrentScope_ShouldTakeFromCurrentScope()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterScope<ScopeB>(configurator =>
        {
            configurator.Register<ScopeBDependency>();
        });

        var container = containerBuilder.Build();

        // Asserts
        var scope = container.Resolve<ScopeB>();

        scope.Dependency.Should().BeOfType<ScopeBDependency>();
    }
    
    [Test]
    public void RequestedDependency_WhenNotExist_ThrowProperException()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterScope<ScopeB>(configurator => { });

        // We don't throw exception on building because scoped container will be built on resolve
        var container = containerBuilder.Build();

        // Asserts
        container
            .Invoking(subject => subject.Resolve<ScopeB>())
            .Should()
            .Throw<SparseInjectException>();
    }

    private class ParentScopeWithInnerScopes : Scope
    {
        public ScopeA ScopeA { get; }
        public ScopeB ScopeB { get; }

        public ParentScopeWithInnerScopes(ScopeA scopeA, ScopeB scopeB)
        {
            ScopeA = scopeA;
            ScopeB = scopeB;
        }
    }
    
    [Test]
    public void InnerScopesRegisteredInParentScopes_WhenResolved_ContainCorrectInnerScopes()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterScope<ParentScopeWithInnerScopes>(configurator =>
        {
            configurator.RegisterScope<ScopeA>(_ => { });
            configurator.RegisterScope<ScopeB>(configurator =>
            {
                configurator.Register<ScopeBDependency>();
            });
        });
        
        var container = containerBuilder.Build();
        
        // Asserts
        var scope = container.Resolve<ParentScopeWithInnerScopes>();
        
        scope.ScopeA.Should().BeOfType<ScopeA>();
        scope.ScopeB.Should().BeOfType<ScopeB>();
    }
    
    [Test]
    public void InnerScopesRegisteredInParentScopesAndInnerDependencyInMainScope_WhenResolved_ContainCorrectInnerScopes()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<ScopeBDependency>();
        containerBuilder.RegisterScope<ParentScopeWithInnerScopes>(configurator =>
        {
            configurator.RegisterScope<ScopeA>(_ => { });
            configurator.RegisterScope<ScopeB>(_ => { });
        });
        
        var container = containerBuilder.Build();
        
        // Asserts
        var scope = container.Resolve<ParentScopeWithInnerScopes>();
        
        scope.ScopeA.Should().BeOfType<ScopeA>();
        scope.ScopeB.Should().BeOfType<ScopeB>();
    }
    
    [Test]
    public void InnerScopesRegisteredInMainScope_WhenScopeWithInnersResolved_ContainCorrectInnerScopes()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.RegisterScope<ParentScopeWithInnerScopes>(_ => { });
        containerBuilder.RegisterScope<ScopeA>(_ => { });
        containerBuilder.RegisterScope<ScopeB>(configurator =>
        {
            configurator.Register<ScopeBDependency>();
        });
        
        var container = containerBuilder.Build();
        
        // Asserts
        var scope = container.Resolve<ParentScopeWithInnerScopes>();
        
        scope.ScopeA.Should().BeOfType<ScopeA>();
        scope.ScopeB.Should().BeOfType<ScopeB>();
    }
    
    [Test]
    public void InnerScopesDependencyRegisteredInMainScope_WhenScopeWithInnersResolved_ContainCorrectInnerScopes()
    {
        // Setup
        var containerBuilder = new ContainerBuilder();
        
        containerBuilder.Register<ScopeBDependency>();
        containerBuilder.RegisterScope<ParentScopeWithInnerScopes>(_ => { });
        containerBuilder.RegisterScope<ScopeA>(_ => { });
        containerBuilder.RegisterScope<ScopeB>(_ => { });
        
        var container = containerBuilder.Build();
        
        // Asserts
        var scope = container.Resolve<ParentScopeWithInnerScopes>();
        
        scope.ScopeA.Should().BeOfType<ScopeA>();
        scope.ScopeB.Should().BeOfType<ScopeB>();
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
    
    // TODO: Add inherited scopes tests
}