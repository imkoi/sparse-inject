using System;

namespace SparseInject.ReflectionBaking.Tests.Generic
{
    public class GenericTestInstaller
    {
        public static void Install(IScopeBuilder scopeBuilder)
        {
            scopeBuilder.Register<GenericDependencyA<string>>();
            scopeBuilder.Register<IGenericDependencyB, GenericDependencyB<string>>();
            scopeBuilder.Register<IGenericDependencyC0, IGenericDependencyC1, GenericDependencyC<string>>();
            scopeBuilder.Register<IGenericDependencyD0, IGenericDependencyD1, IGenericDependencyD2, GenericDependencyD<string>>();

            scopeBuilder.Register<ManyGenerics<string, int, IDisposable>>();
            
            //TODO: fix this case
            scopeBuilder.Register<ManyGenerics<string, int, GenericDependencyA<string>>>();
        }
    }
    
    public class GenericDependencyA<T> { }

    public interface IGenericDependencyB { }
    public class GenericDependencyB<T> : IGenericDependencyB { }

    public interface IGenericDependencyC0 { }
    public interface IGenericDependencyC1 { }
    public class GenericDependencyC<T> : IGenericDependencyC0, IGenericDependencyC1 { }

    public interface IGenericDependencyD0 { }
    public interface IGenericDependencyD1 { }
    public interface IGenericDependencyD2 { }
    public class GenericDependencyD<T> : IGenericDependencyD0, IGenericDependencyD1, IGenericDependencyD2 { }
    
    public class ManyGenerics<T0, T1, T2> { }
}