namespace SparseInject.ReflectionBaking.Tests.NestedClass;

public class NestedClassTestInstaller
{
    public static void Install(IScopeBuilder scopeBuilder)
    {
        scopeBuilder.Register<TransientDependencyA>();
        scopeBuilder.Register<ITransientDependencyB, TransientDependencyB>();
        scopeBuilder.Register<ITransientDependencyC0, ITransientDependencyC1, TransientDependencyC>();
        scopeBuilder.Register<ITransientDependencyD0, ITransientDependencyD1, ITransientDependencyD2, TransientDependencyD>();
    }
    
    public class TransientDependencyA { }

    public interface ITransientDependencyB { }
    public class TransientDependencyB : ITransientDependencyB { }

    public interface ITransientDependencyC0 { }
    public interface ITransientDependencyC1 { }
    public class TransientDependencyC : ITransientDependencyC0, ITransientDependencyC1 { }

    public interface ITransientDependencyD0 { }
    public interface ITransientDependencyD1 { }
    public interface ITransientDependencyD2 { }
    public class TransientDependencyD : ITransientDependencyD0, ITransientDependencyD1, ITransientDependencyD2 { }
}