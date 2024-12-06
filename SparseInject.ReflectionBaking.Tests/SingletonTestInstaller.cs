namespace SparseInject.ReflectionBaking.Tests.Singleton;

public static class SingletonTestInstaller
{
    public static void Install(IScopeBuilder scopeBuilder)
    {
        scopeBuilder.Register<SingletonDependencyA>(Lifetime.Singleton);
        scopeBuilder.Register<ISingletonDependencyB, SingletonDependencyB>(Lifetime.Singleton);
        scopeBuilder.Register<ISingletonDependencyC0, ISingletonDependencyC1, SingletonDependencyC>(Lifetime.Singleton);
        scopeBuilder.Register<ISingletonDependencyD0, ISingletonDependencyD1, ISingletonDependencyD2, SingletonDependencyD>(Lifetime.Singleton);
    }
}

public class SingletonDependencyA { }

public interface ISingletonDependencyB { }
public class SingletonDependencyB : ISingletonDependencyB { }

public interface ISingletonDependencyC0 { }
public interface ISingletonDependencyC1 { }
public class SingletonDependencyC : ISingletonDependencyC0, ISingletonDependencyC1 { }

public interface ISingletonDependencyD0 { }
public interface ISingletonDependencyD1 { }
public interface ISingletonDependencyD2 { }
public class SingletonDependencyD : ISingletonDependencyD0, ISingletonDependencyD1, ISingletonDependencyD2 { }