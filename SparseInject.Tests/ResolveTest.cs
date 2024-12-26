using FluentAssertions;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class ResolveTest
{
    private class ScopeA : Scope { }
    private class ScopeB : Scope { }
    private class ScopeC : Scope { }
    private class MainScopeDependency { }
    
    [Test]
    public void RegisteredScope_WhenScopeResolveInstanceFromParent_ReturnCorrectInstance()
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.Register<MainScopeDependency>();
        containerBuilder.RegisterScope<ScopeA>(_ => { });
        
        // Asserts
        var container = containerBuilder.Build();
        
        var scope = container.Resolve<ScopeA>();

        scope._container.Resolve<MainScopeDependency>().Should().BeOfType<MainScopeDependency>();
    }
    
    [Ignore("Need fixes in core")]
    [Test]
    public void RegisteredInnerScopes_WhenLastScopeResolveInstanceFromMain_ReturnCorrectInstance()
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.Register<MainScopeDependency>();
        containerBuilder.RegisterScope<ScopeA>(configuratorA =>
        {
            configuratorA.RegisterScope<ScopeB>(configuratorB =>
            {
                configuratorB.RegisterScope<ScopeC>(_ => { });
            });
        });
        
        // Asserts
        var container = containerBuilder.Build();
        
        var scopeA = container.Resolve<ScopeA>();
        var scopeB = scopeA._container.Resolve<ScopeB>();
        var scopeC = scopeB._container.Resolve<ScopeC>();

        scopeC._container.Resolve<MainScopeDependency>().Should().BeOfType<MainScopeDependency>();
    }
}