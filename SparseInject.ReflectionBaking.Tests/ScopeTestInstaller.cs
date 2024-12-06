namespace SparseInject.ReflectionBaking.Tests.Scope;

public static class ScopeTestInstaller
{
    public static void Install(IScopeBuilder scopeBuilder)
    {
        scopeBuilder.Register<Dependency>();
        
        scopeBuilder.RegisterScope<ScopeA>(scopeA =>
        {
            scopeA.Register<DependencyA>();
            scopeA.Register<DependencyA>();
            scopeA.Register<DependencyA>();
        });
        
        scopeBuilder.RegisterScope<IScopeB, ScopeB>(scopeB =>
        {
            scopeB.Register<DependencyB>();
            scopeB.Register<DependencyB>();
            scopeB.Register<DependencyB>();
        });
        
        scopeBuilder.RegisterScope<ScopeC>((scopeC, parentScopeC) =>
        {
            scopeC.Register<DependencyC>();
            scopeC.Register<DependencyC>();
            scopeC.Register<DependencyC>();
        });
        
        scopeBuilder.RegisterScope<IScopeD, ScopeD>((scopeD, parentScopeD) =>
        {
            scopeD.Register<DependencyD>();
            scopeD.Register<DependencyD>();
            scopeD.Register<DependencyD>();
        });
    }
}

public interface IScopeB : IDisposable { }
public interface IScopeD : IDisposable { }

public class ScopeA : SparseInject.Scope { }
public class ScopeB : SparseInject.Scope, IScopeB { }
public class ScopeC : SparseInject.Scope { }
public class ScopeD : SparseInject.Scope, IScopeD { }

public class Dependency { }
public class DependencyA { }
public class DependencyB { }
public class DependencyC { }
public class DependencyD { }