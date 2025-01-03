using System;
using FluentAssertions;
using NUnit.Framework;
using SparseInject;

[TestFixture]
public class ResolveTest
{
    private class ScopeA : Scope { }
    private class ScopeB : Scope { }
    private class ScopeC : Scope { }
    private class ScopeD : Scope { public ScopeD(MainScopeDependency mainScopeDependency) { } }
    private class ScopeE : Scope { public ScopeE(MainScopeDependency[] mainScopeDependency) { } }
    private class ScopeF : Scope { public ScopeF(IDisposable[] disposables) { } }
    private class ScopeG : Scope { public ScopeG(IDisposable disposable) { } }
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
    
    [Test]
    public void RegisteredInnerScopes_WhenLastScopeWithInstanceFromMainResolve_ReturnCorrectInstance()
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.Register<MainScopeDependency>();
        containerBuilder.RegisterScope<ScopeA>(configuratorA =>
        {
            configuratorA.RegisterScope<ScopeB>(configuratorB =>
            {
                configuratorB.RegisterScope<ScopeC>(configuratorC =>
                {
                    configuratorC.RegisterScope<ScopeD>(_ => { });
                });
            });
        });
        
        // Asserts
        var container = containerBuilder.Build();
        
        var scopeA = container.Resolve<ScopeA>();
        var scopeB = scopeA._container.Resolve<ScopeB>();
        var scopeC = scopeB._container.Resolve<ScopeC>();
        
        scopeC._container.Invoking(subject => subject.Resolve<ScopeD>()).Should().NotThrow();
    }
    
    [Test]
    public void RegisteredInnerScopes_WhenLastScopeWithRegisteredCollectionResolve_ReturnCorrectInstance()
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.Register<MainScopeDependency>();
        containerBuilder.RegisterScope<ScopeA>(configuratorA =>
        {
            configuratorA.RegisterScope<ScopeB>(configuratorB =>
            {
                configuratorB.RegisterScope<ScopeC>(configuratorC =>
                {
                    configuratorC.RegisterScope<ScopeD>(configuratorD =>
                    {
                        configuratorD.RegisterScope<ScopeE>(_ => { });
                    });
                });
            });
        });
        
        // Asserts
        var container = containerBuilder.Build();
        
        var scopeA = container.Resolve<ScopeA>();
        var scopeB = scopeA._container.Resolve<ScopeB>();
        var scopeC = scopeB._container.Resolve<ScopeC>();
        var scopeD = scopeC._container.Resolve<ScopeD>();
        
        scopeD._container.Invoking(subject => subject.Resolve<ScopeE>()).Should().NotThrow();
    }
    
    [Test]
    public void RegisteredInnerScopes_WhenLastScopeWithNotRegisteredArrayResolve_ReturnCorrectInstance()
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.Register<MainScopeDependency>();
        containerBuilder.RegisterScope<ScopeA>(configuratorA =>
        {
            configuratorA.RegisterScope<ScopeB>(configuratorB =>
            {
                configuratorB.RegisterScope<ScopeC>(configuratorC =>
                {
                    configuratorC.RegisterScope<ScopeD>(configuratorD =>
                    {
                        configuratorD.RegisterScope<ScopeE>(configuratorE =>
                        {
                            configuratorE.RegisterScope<ScopeF>(_ => { });
                        });
                    });
                });
            });
        });
        
        // Asserts
        var container = containerBuilder.Build();
        
        var scopeA = container.Resolve<ScopeA>();
        var scopeB = scopeA._container.Resolve<ScopeB>();
        var scopeC = scopeB._container.Resolve<ScopeC>();
        var scopeD = scopeC._container.Resolve<ScopeD>();
        var scopeE = scopeD._container.Resolve<ScopeE>();
        
        scopeE._container.Invoking(subject => subject.Resolve<ScopeF>()).Should().NotThrow();
    }
    
    [Test]
    public void RegisteredInnerScopes_WhenLastScopeWithNotRegisteredDependencyResolve_ReturnCorrectInstance()
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.Register<MainScopeDependency>();
        containerBuilder.RegisterScope<ScopeA>(configuratorA =>
        {
            configuratorA.RegisterScope<ScopeB>(configuratorB =>
            {
                configuratorB.RegisterScope<ScopeC>(configuratorC =>
                {
                    configuratorC.RegisterScope<ScopeD>(configuratorD =>
                    {
                        configuratorD.RegisterScope<ScopeE>(configuratorE =>
                        {
                            configuratorE.RegisterScope<ScopeF>(_ => { });
                            configuratorE.RegisterScope<ScopeG>(_ => { });
                        });
                    });
                });
            });
        });
        
        // Asserts
        var container = containerBuilder.Build();
        
        var scopeA = container.Resolve<ScopeA>();
        var scopeB = scopeA._container.Resolve<ScopeB>();
        var scopeC = scopeB._container.Resolve<ScopeC>();
        var scopeD = scopeC._container.Resolve<ScopeD>();
        var scopeE = scopeD._container.Resolve<ScopeE>();

        scopeE._container.Invoking(subject => subject.Resolve<ScopeG>()).Should().Throw<SparseInjectException>();
    }
    
    [Test]
    public void RegisteredMiddleScopes_WhenLastScopeWithNotRegisteredDependencyResolve_ReturnCorrectInstance()
    {
        var containerBuilder = new ContainerBuilder();

        containerBuilder.Register<MainScopeDependency>();
        containerBuilder.RegisterScope<ScopeA>(configuratorA =>
        {
            configuratorA.RegisterScope<ScopeB>(configuratorB =>
            {
                configuratorB.RegisterScope<ScopeG>(_ => { });
                configuratorB.RegisterScope<ScopeC>(configuratorC =>
                {
                    configuratorC.RegisterScope<ScopeD>(configuratorD =>
                    {
                        configuratorD.RegisterScope<ScopeE>(configuratorE =>
                        {
                            configuratorE.RegisterScope<ScopeF>(_ => { });
                        });
                    });
                });
            });
        });
        
        // Asserts
        var container = containerBuilder.Build();
        
        var scopeA = container.Resolve<ScopeA>();
        var scopeB = scopeA._container.Resolve<ScopeB>();
        var scopeC = scopeB._container.Resolve<ScopeC>();
        var scopeD = scopeC._container.Resolve<ScopeD>();
        var scopeE = scopeD._container.Resolve<ScopeE>();

        scopeE._container.Invoking(subject => subject.Resolve<ScopeG>()).Should().Throw<SparseInjectException>();
    }
}